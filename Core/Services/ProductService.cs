using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Context;
using Datalayer.Entities;
using Datalayer.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = Datalayer.Entities.Attribute;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        IUserService _userService;
        IToolsService _toolsService;
        ISettingService _settingService;
        private ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context, IUserService userService, IToolsService toolsService, ISettingService settingService)
        {
            _context = context;
            _userService = userService;
            _toolsService = toolsService;
            _settingService = settingService;
        }

        public List<Product> GetAllProduct(int number = -1)
        {
            if (number == -1)
            {
                return _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).Include(a => a.Photo).ToList();
            }
            else
            {
                return _context.Products.Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).Include(a => a.Photo).Take(number).ToList();
            }
        }

        public List<Product> GetAllProduct(string search)
        {
            return _context.Products.Where(p => !p.IsDeleted && p.Title.Contains(search)).OrderByDescending(p => p.Id).Include(a => a.Photo).ToList();
        }

        public List<Product> GetAllPublishedProduct(int number = 0, bool IsOnlyAvailable = false)
        {
            List<Product> FilteredProducts = new List<Product>();

            if (number == 0)
            {
                FilteredProducts = _context.Products.Where(p => p.Status == "Publish" && !p.IsDeleted && !p.IsHidden).OrderByDescending(p => p.Id).Include(a => a.Photo).Include(a => a.Variables).ToList();
            }
            else
            {
                FilteredProducts = _context.Products.Where(p => p.Status == "Publish" && !p.IsDeleted && !p.IsHidden).OrderByDescending(p => p.Id).Include(a => a.Photo).Include(a => a.Variables).Take(number).ToList();
            }

            if (IsOnlyAvailable)
                return FilteredProducts.Where(p => p.IsInStock).ToList();

            return FilteredProducts;
        }

        public List<Product> GetAllPublishedProduct(string search)
        {
            var searchTerms = search.Split(" ");

            return _context.Products.Where(p => p.Status == "Publish" && !p.IsDeleted && !p.IsHidden && searchTerms.All(s => p.Title.Contains(s) || p.EnglishTitle.Contains(s))).Include(a => a.Photo).ToList();
        }

        public List<Media> GetGalleryPhotos(string Photos)
        {
            var PhotoArray = Photos.Split(',');
            var result = new List<Media>();
            foreach (var item in PhotoArray)
            {
                var photo = _context.Medias.FirstOrDefault(p => p.Id == int.Parse(item));
                if (photo != null)
                    result.Add(photo);
            }

            return result;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Include(p => p.Photo).Include(v => v.Variables).Where(p => p.Id == id).FirstOrDefault();
        }

        public Product GetProduct(string slug)
        {
            return _context.Products.Include(p => p.Photo).Where(p => p.Slug == slug).FirstOrDefault();
        }

        public List<Product> GetSpecificProduct(int page, int countPerPage, string? search)
        {
            if (search == null)
            {
                return _context.Products.Where(p => !p.IsDeleted).Include(a => a.Photo).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

            }
            else
            {
                return _context.Products.Where(p => !p.IsDeleted && p.Title.Contains(search)).Include(a => a.Photo).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

            }

        }

        public string GetUnicSlug(string slug, int id)
        {
            bool isSlugExist = false;
            while (!isSlugExist)
            {
                var q = from a in _context.Products where a.Slug == slug select a;
                if (q.Count() > 0)
                {
                    if (q.Count() == 1 && q.FirstOrDefault().Id == id)
                    {
                        isSlugExist = true;
                    }
                    else
                    {
                        slug = slug + "-1";
                        isSlugExist = false;
                    }
                }
                else
                {
                    isSlugExist = true;
                }
            }

            return slug;

        }

        public Variable? GetSelectedVariable(int variableId)
        {
            return _context.Variables.FirstOrDefault(v => v.Id == variableId);

        }

        public void AddVariableProductToOrder(int VariableId, int OrderId, int number)
        {
            OrderProduct op = new OrderProduct()
            {
                number = number,
                OrderId = OrderId,
                VariableId = VariableId,
                ProductId = GetSelectedVariable(VariableId).ProductId
            };

            _context.Add(op);
            _context.SaveChanges();
        }

        public void AddCustomVariableProductToOrder(OrderProduct op)
        {
            _context.Add(op);
            _context.SaveChanges();
        }

        public bool isVarProductExistInOrder(int VariableId, int OrderId)
        {
            return _context.OrderProducts.Where(o => o.OrderId == OrderId && o.VariableId == VariableId).Any();
        }

        public List<OrderProduct> GetProductsInOrder(int orderId)
        {
            return _context.OrderProducts.Where(o => o.OrderId == orderId).Include(p => p.Product).ThenInclude(p => p.Photo).Include(v => v.Variable).ThenInclude(v => v.Product).ThenInclude(v => v.Photo).Include(p => p.Order).ToList();
        }

        public int GetTotalPriceInOrder(int OrderId)
        {
            int total = 0;
            var order = GetOrderById(OrderId);
            if (order.IsFinal != true)
            {
                var orderProducts = _context.OrderProducts.Include(o => o.Variable).Include(o => o.Product).Where(o => o.OrderId == OrderId && o.Variable != null);

                foreach (var item in orderProducts)
                {

                    if (item.Variable.SalePrice != null)
                    {
                        total += (item.Variable.SalePrice.Value * item.number);
                    }
                    else
                    {
                        total += (item.Variable.Price.Value * item.number);
                    }
                }

                if (order.ShippingId != null)
                {
                    if (order.Shipping.ShipPrice > 0)
                        total += order.Shipping.ShipPrice;
                }
            }
            else
            {
                var orderProducts = _context.OrderProducts.Include(o => o.Variable).Include(o => o.Product).Where(o => o.OrderId == OrderId);

                foreach (var item in orderProducts)
                {
                    total += (item.Price * item.number);
                }

                if (order.ShippingPrice > 0)
                {
                    total += order.ShippingPrice;
                }
            }



            return total;
        }

        public int GetTotalRegularPriceInOrder(int OrderId)
        {
            int total = 0;
            var order = GetOrderById(OrderId);
            var orderProducts = _context.OrderProducts.Include(o => o.Variable).Where(o => o.OrderId == OrderId && o.Variable != null);

            foreach (var item in orderProducts)
            {
                total += (item.Variable.Price.Value * item.number);
            }

            if (order.ShippingId != null)
            {
                if (order.Shipping.ShipPrice > 0)
                    total += order.Shipping.ShipPrice;
            }

            return total;
        }

        public int GetTotalPriceWhenVerify(int OrderId)
        {
            int total = 0;
            var orderProducts = _context.OrderProducts.Where(o => o.OrderId == OrderId);
            var order = GetOrderById(OrderId);

            foreach (var item in orderProducts)
            {
                total += (item.Price * item.number);
            }

            if (order.ShippingPrice > 0)
            {
                total += order.ShippingPrice;
            }

            return total;
        }

        public void SaveDatabse()
        {
            _context.SaveChanges();
        }

        public void SaveProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public int GetTotalNumberInOrder(int OrderId)
        {
            int number = 0;
            var orderProducts = _context.OrderProducts.Where(o => o.OrderId == OrderId && o.VariableId != null);
            foreach (var item in orderProducts)
            {
                number += item.number;
            }

            return number;
        }

        public void DeleteProductFromOrder(int OrderProductId)
        {
            var orderProduct = _context.OrderProducts.FirstOrDefault(o => o.Id == OrderProductId);

            if (orderProduct != null)
            {

                _context.OrderProducts.Remove(orderProduct);

                _context.SaveChanges();
            }

        }

        public void DeleteEmptyOrder(int orderId)
        {
            var ValidProductCount = _context.OrderProducts.Where(o => o.OrderId == orderId && o.Variable != null).Count();
            if (ValidProductCount == 0)
            {
                var order = GetOrderById(orderId);

                _context.Orders.Remove(order);
            }

            _context.SaveChanges();
        }

        public Order GetOrderById(int OrderId)
        {
            return _context.Orders.Include(p => p.User).Include(p => p.Shipping).Include(p => p.Coupon).FirstOrDefault(o => o.Id == OrderId);
        }
        public OrderProduct GetOrderProductById(int OrderProductId)
        {
            return _context.OrderProducts.Include(o => o.Variable).Include(o => o.Product).ThenInclude(o => o.Photo).FirstOrDefault(o => o.Id == OrderProductId);
        }

        public OrderProduct ChangeProductNumberInCart(int OrderProductId, int number)
        {
            var openPackage = GetOrderProductById(OrderProductId);
            var NumberStock = openPackage.Variable.NumberInStock.Value;

            var result = openPackage.number + number;

            if (result > 0 && result <= NumberStock)
            {
                openPackage.number = result;
                _context.SaveChanges();
            }

            return openPackage;
        }

        public List<Shipping> GetAllShippingMethods()
        {
            return _context.Shippings.ToList();
        }

        //public int GetShippingPrice(int OrderId)
        //{
        //    var order = GetOrderById(OrderId);
        //    if (order.IsFinal != null)
        //        return _context.Orders.Include(p => p.Shipping).FirstOrDefault(o => o.Id == OrderId).Shipping.ShipPrice;
        //    else
        //        return _context.Orders.Include(p => p.Shipping).FirstOrDefault(o => o.Id == OrderId).ShippingPrice;

        //}

        public void SetShippingPrice(Order order, int shippMethodId)
        {
            order.ShippingId = shippMethodId;
            _context.SaveChanges();

        }

        public List<City> getStates()
        {
            return _context.Cities.Where(c => c.ParentId == null).ToList();
        }

        public List<City> getCityOfState(int StateId)
        {
            return _context.Cities.Where(c => c.ParentId == StateId).ToList();

        }

        public int GetStateOfCity(int cityId)
        {
            return _context.Cities.FirstOrDefault(c => c.Id == cityId).ParentId.Value;
        }

        public void SetCheckout(Order order, string phoneNumber, string UserDeliverName, string UserDeliverKodeMelli, string UserPhoneNumber, string UserHomePhone, int City, string Address, string PostalCode, string Description)
        {
            order.PhoneNumber = UserPhoneNumber;
            order.HomePhone = UserHomePhone;
            order.Address = Address;
            order.MelliCode = UserDeliverKodeMelli;
            order.Name = UserDeliverName;
            order.CityId = City;
            order.PostalCode = PostalCode;
            order.Description = Description;

            _context.SaveChanges();
        }

        public string VerifyProductCountInCart(OrderProduct OrderProdcut)
        {
            if (OrderProdcut.number > OrderProdcut.Variable.NumberInStock)
            {
                if (OrderProdcut.Variable.NumberInStock == 0)
                {
                    _context.OrderProducts.Remove(OrderProdcut);
                    _context.SaveChanges();
                    return "متاسفانه محصول " + OrderProdcut.Product.Title + " ناموجود شده است و از سبد خرید شما حذف شد";
                }
                else
                {
                    OrderProdcut.number = OrderProdcut.Variable.NumberInStock.Value;
                    _context.SaveChanges();
                    return "متاسفانه تعداد محصول " + OrderProdcut.Product.Title + " از تعداد موجودی بیشتر است (موجودی: " + OrderProdcut.Variable.NumberInStock + " عدد)";
                }


            }
            return "1";
        }

        public string VerifyCouponInCart(int orderId)
        {
            string Error = string.Empty;

            var order = GetOrderById(orderId);
            var coupon = order.Coupon;

            var user = order.User;
            var isUserUsedCoupon = IsUserUsedCoupon(user.Id, coupon.Id);

            if (isUserUsedCoupon && coupon.AnyUserOneTime == true)
            {
                Error = "از این کد تخفیف فقط یک بار میتوانید استفاده کنید";
            }

            bool isHaveFinalOrder = _userService.IsUserHaveFinalOrder(user.Id);
            if (isHaveFinalOrder && coupon.OnlyFirstOrder)
            {
                Error = "از این کد تخفیف فقط برای سفارش اول می توانید استفاده کنید";
            }

            if (coupon.StartDate.HasValue)
            {
                PersianCalendar pc = new PersianCalendar();
                var savedDate = coupon.StartDate.Value;
                DateTime dt = new DateTime(savedDate.Year, savedDate.Month, savedDate.Day, savedDate.Hour, savedDate.Minute, savedDate.Second, pc);

                if (DateTime.Now < dt)
                {
                    Error = "این کد تخفیف فعال نیست";
                }
            }

            if (coupon.EndDate.HasValue)
            {
                PersianCalendar pc = new PersianCalendar();
                var savedDate = coupon.EndDate.Value;
                DateTime dt = new DateTime(savedDate.Year, savedDate.Month, savedDate.Day, savedDate.Hour, savedDate.Minute, savedDate.Second, pc);

                if (DateTime.Now > dt)
                {
                    Error = "کد تخفیف منقضی شده است";
                }
            }

            if (coupon.MinTotalOrder.HasValue)
            {
                if (GetTotalPriceInOrder(order.Id) < coupon.MinTotalOrder.Value)
                {
                    Error = "برای استفاده از این کد تخفیف مجموع سبد خرید شما باید بالای " + coupon.MinTotalOrder.Value.ToString("#,0") + " تومان باشد";
                }
            }

            if (!string.IsNullOrEmpty(Error))
            {
                order.CouponId = null;
                _context.SaveChanges();

                return Error + " (کد تخفیف حذف شد!)";
            }

            return "OK";

        }

        public void PrepareOrderBeforePayment(Order order)
        {
            var ProductOrders = _context.OrderProducts.Where(o => o.OrderId == order.Id && o.VariableId != null);
            order.ShippingPrice = order.Shipping.ShipPrice;
            foreach (var po in ProductOrders)
            {
                po.Price = (po.Variable.SalePrice != null) ? po.Variable.SalePrice.Value : po.Variable.Price.Value;
                po.VariableTitle = po.Variable.Title;
                po.Variable.NumberInStock -= po.number;
            }

            order.IsReservedStack = true;

            if (order.CouponId != null)
            {
                order.CouponName = order.Coupon.Code;
                order.OffPercent = order.Coupon.Percent;
                order.CouponOffValue = CalculateOffValue(order, GetTotalPriceInOrder(order.Id));
            }

            order.SudeShoma = GetTotalRegularPriceInOrder(order.Id) - GetTotalPriceInOrder(order.Id);

            _context.SaveChanges();
        }

        public void RefundOrderAfterFailedPayment(Order order)
        {
            if (order.IsReservedStack)
            {
                var ProductOrders = _context.OrderProducts.Include(v => v.Variable).Where(o => o.OrderId == order.Id && o.VariableId != null);
                foreach (var po in ProductOrders)
                {
                    po.Variable.NumberInStock += po.number;
                }

                order.IsReservedStack = false;

                _context.SaveChanges();
            }

        }
        public void CloseOrder(Order order, string paymentInfo)
        {
            order.IsFinal = true;
            order.IsReservedStack = false;
            order.ShippingId = null;

            order.User.FirstName = order.Name;

            order.Status = "NoShipping";

            var ProductOrders = _context.OrderProducts.Include(v => v.Variable).Include(p => p.Product).Where(o => o.OrderId == order.Id && o.VariableId != null);
            foreach (var item in ProductOrders)
            {
                item.VariableId = null;
                item.Product.NumberSale += item.number;
            }

            order.PaymentCode = paymentInfo;
            order.PaymentDate = DateTime.Now;
            order.CreationDate = DateTime.Now;

            if (order.CouponId != null)
            {
                _context.UserCoupons.Add(new UserCoupon { UserId = order.UserId, CouponId = order.CouponId.Value });

                order.CouponId = null;
            }

            var smsText = _settingService.GetSettingValue("SMSText").Replace("[Name]", order.Name).Replace("[OrderNumber]", order.Id.ToString());
            _toolsService.SendSMS(smsText, order.User.PhoneNumber);


            _context.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Where(o => o.IsFinal == true).ToList();
        }

        public List<Order> GetSpecificOrders(int page, int countPerPage)
        {
            return _context.Orders.Where(o => o.IsFinal == true).Include(o => o.User).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public City GetCityById(int cityId)
        {
            return _context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public void SetStatusSelected(Order order, string status, string TrackingCode)
        {
            order.Status = status;
            order.TrackingCode = TrackingCode;
            if (status == "Completed")
            {
                var smsText = _settingService.GetSettingValue("SMSTextCompleted").Replace("[Name]", order.Name).Replace("[OrderNumber]", order.Id.ToString()).Replace("[TrackCode]", TrackingCode);
                _toolsService.SendSMS(smsText, order.User.PhoneNumber);
            }
            else if (status == "Canceled")
            {
                var smsText = _settingService.GetSettingValue("SMSTextCanceled").Replace("[Name]", order.Name).Replace("[OrderNumber]", order.Id.ToString());
                _toolsService.SendSMS(smsText, order.User.PhoneNumber);
            }
            else if (status == "Refund")
            {
                var smsText = _settingService.GetSettingValue("SMSTextRefund").Replace("[Name]", order.Name).Replace("[OrderNumber]", order.Id.ToString());
                _toolsService.SendSMS(smsText, order.User.PhoneNumber);
            }
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var productOrders = _context.OrderProducts.Where(o => o.OrderId == id);

            foreach (var item in productOrders)
            {
                _context.OrderProducts.Remove(item);
            }

            var order = _context.Orders.Where(o => o.Id == id).FirstOrDefault();

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void DeleteProduct(int Id)
        {
            var orderProductsInCart = _context.OrderProducts.Where(p => p.ProductId == Id && p.Order.IsFinal == false);
            foreach (var item in orderProductsInCart)
            {
                _context.OrderProducts.Remove(item);
            }

            GetProduct(Id).IsDeleted = true;

            _context.SaveChanges();
        }

        public List<Coupon> GetSpecificCouponCodes(int page, int countPerPage)
        {
            return _context.Coupons.Where(c => c.isDeleted == false).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public List<Coupon> GetAllCoupons()
        {
            return _context.Coupons.Where(c => c.isDeleted == false).ToList();
        }

        public Coupon GetCoupon(int CouponId)
        {
            return _context.Coupons.FirstOrDefault(c => c.Id == CouponId);
        }

        public int AddCouponCode(string Code, int CodePercent, int? MaxValue, DateTime? StartDate, DateTime? EndDate, int? MinTotal, bool onlyFirstOrder, bool AnyUserOneTime)
        {
            Coupon coupon = new Coupon();
            coupon.Code = Code;
            coupon.Percent = CodePercent;
            coupon.MaxValue = MaxValue;
            coupon.StartDate = StartDate;
            coupon.EndDate = EndDate;
            coupon.MinTotalOrder = MinTotal;
            coupon.OnlyFirstOrder = onlyFirstOrder;
            coupon.AnyUserOneTime = AnyUserOneTime;
            _context.Coupons.Add(coupon);

            _context.SaveChanges();

            return coupon.Id;

        }

        public void UpdateCouponCode(int CouponId, int CodePercent, int? MaxValue, DateTime? StartDate, DateTime? EndDate, int? MinTotal, bool onlyFirstOrder, bool AnyUserOneTime)
        {
            var oldCoupon = GetCoupon(CouponId);

            //var ordersWithCoupon = _context.Orders.Where(c => c.CouponId == CouponId);

            //foreach (var order in ordersWithCoupon)
            //{
            //    order.CouponId = null;
            //}

            oldCoupon.Percent = CodePercent;
            oldCoupon.MaxValue = MaxValue;
            oldCoupon.StartDate = StartDate;
            oldCoupon.EndDate = EndDate;
            oldCoupon.MinTotalOrder = MinTotal;
            oldCoupon.OnlyFirstOrder = onlyFirstOrder;
            oldCoupon.AnyUserOneTime = AnyUserOneTime;

            _context.SaveChanges();
        }

        public Coupon? IsExistCoupon(string code)
        {
            return _context.Coupons.FirstOrDefault(c => c.Code == code);
        }

        public int GetTotalCouponUsed(int CouponId)
        {
            return _context.UserCoupons.Where(c => c.CouponId == CouponId).Count();
        }

        public void DeleteCoupon(int CouponId)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == CouponId);

            var ordersWithCoupon = _context.Orders.Where(c => c.CouponId == CouponId);
            foreach (var order in ordersWithCoupon)
            {
                order.CouponId = null;
            }

            _context.Coupons.Remove(coupon);
            _context.SaveChanges();
        }

        public bool IsUserUsedCoupon(int userId, int couponId)
        {
            return _context.UserCoupons.Where(u => u.UserId == userId && u.CouponId == couponId).Any();
        }

        public int CalculateOffValue(Order order, int TotalPrice)
        {
            int offValue = 0;
            if (order.CouponId != null)
            {
                offValue = (order.Coupon.Percent * TotalPrice) / 100;
                if (order.Coupon.MaxValue.HasValue)
                {
                    if (offValue > order.Coupon.MaxValue.Value)
                    {
                        offValue = order.Coupon.MaxValue.Value;
                    }
                }
            }

            return offValue;
        }

        //public void SendNitifSms(int productId)
        //{
        //    var product = GetProduct(productId);
        //    string message = "محصول " + product.Title + " موجود شده است برای خرید به پرو ابزار مراجعه فرمایید";

        //    var notifications = _context.Notifications.Where(n => n.ProductId == productId);
        //    string[] numbers = notifications.Select(x=>x.User.MobileNumber).ToArray();

        //    _toolsService.SendManySms(message, numbers);

        //    foreach (var notif in notifications)
        //    {
        //        _context.Notifications.Remove(notif);
        //    }

        //    _context.SaveChanges();

        //}

        public Variable GetMainVariable(int productId)
        {
            List<Variable> pv = new List<Variable>();

            var q = (from a in _context.Variables where a.ProductId == productId && a.NumberInStock > 0 select a).ToList();
            var MinPrice = q.OrderBy(x => x.Price).FirstOrDefault();
            if (q != null)
            {
                if (q.Count > 0)
                {
                    foreach (var item in q)
                    {
                        if (item.SalePrice != null)
                        {
                            pv.Add(item);
                        }
                    }

                    if (pv.Count > 0)
                    {
                        var MinSale = pv.OrderBy(x => x.SalePrice).FirstOrDefault();
                        return (MinSale.SalePrice < MinPrice.Price) ? MinSale : MinPrice;
                    }
                    else
                    {
                        return MinPrice;
                    }
                }
            }

            return null;
        }

        public string GetPriceText(Variable? variable)
        {
            return (variable == null) ? "ناموجود" : ((variable.SalePrice != null) ? variable.SalePrice.Value.ToString("#,0") + " تومان " : variable.Price.Value.ToString("#,0") + " تومان");
        }

        public void AddVariableToProduct(int productId, string[] title, string[] description, string[] price, string[] salePrice, string[] neumberInStock)
        {
            if (title != null)
            {
                for (int i = 0; i < title.Length; i++)
                {
                    if (string.IsNullOrEmpty(title[i]) && string.IsNullOrEmpty(description[i]) && string.IsNullOrEmpty(price[i]) && string.IsNullOrEmpty(salePrice[i]) && string.IsNullOrEmpty(neumberInStock[i]))
                    {
                        break;
                    }
                    Variable pv = new Variable()
                    {
                        Title = title[i],
                        Description = description[i],
                        NumberInStock = string.IsNullOrEmpty(neumberInStock[i]) ? null : int.Parse(neumberInStock[i]),
                        Price = string.IsNullOrEmpty(price[i]) ? null : int.Parse(price[i]),
                        SalePrice = string.IsNullOrEmpty(salePrice[i]) ? null : int.Parse(salePrice[i]),
                        ProductId = productId,
                    };

                    _context.Variables.Add(pv);
                }

                _context.SaveChanges();
            }

        }

        public void DeleteProductVariables(int productId)
        {
            var variables = _context.Variables.Where(p => p.ProductId == productId);

            foreach (var item in variables)
            {
                var productOrder = _context.OrderProducts.Where(v => v.VariableId == item.Id).ToList();

                foreach (var po in productOrder)
                {
                    //_context.OrderProducts.Remove(po);
                    po.VariableId = null;
                }

                _context.Variables.Remove(item);
            }
            _context.SaveChanges();
        }

        public List<Variable> GetProductVariables(int productId)
        {
            return _context.Variables.Where(v => v.ProductId == productId).ToList();
        }

        public void UpdateProductVariables(int productId, string[] title, string[] description, string[] price, string[] salePrice, string[] neumberInStock)
        {
            DeleteProductVariables(productId);
            AddVariableToProduct(productId, title, description, price, salePrice, neumberInStock);
        }

        public void AddAttributeToProduct(int productId, string[] selectedAttributes, string[] attributeValues)
        {
            if (selectedAttributes != null && attributeValues != null)
            {
                if (attributeValues.Length > 0 && selectedAttributes.Length > 0)
                {
                    for (int i = 0; i < selectedAttributes.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(selectedAttributes[i]))
                        {
                            Datalayer.Entities.Attribute productAttribute = new Datalayer.Entities.Attribute()
                            {
                                ProductId = productId,
                                AttributeKey = selectedAttributes[i],
                                AttributeValue = attributeValues[i]
                            };

                            _context.Attributes.Add(productAttribute);
                        }

                    }

                    _context.SaveChanges();
                }
            }
        }

        public void DeleteProductAttributes(int productId)
        {
            var Attributes = _context.Attributes.Where(p => p.ProductId == productId);
            foreach (var Attribute in Attributes)
            {
                _context.Attributes.Remove(Attribute);
            }

            _context.SaveChanges();
        }

        public void UpdateProductAttributes(int productId, string[] selectedAttributes, string[] attributeValues)
        {
            DeleteProductAttributes(productId);
            AddAttributeToProduct(productId, selectedAttributes, attributeValues);
        }

        public List<Attribute> GetProductAttributes(int productId)
        {
            return _context.Attributes.Where(p => p.ProductId == productId).ToList();
        }

        public void AddCategoryToProduct(int productId, string[] categories)
        {
            foreach (var cat in categories)
            {
                ProductCategory productCat = new ProductCategory()
                {
                    ProductId = productId,
                    CategoryId = int.Parse(cat),
                };

                _context.ProductCategories.Add(productCat);
            }

            _context.SaveChanges();

        }

        public void DeleteProductCategory(int productId)
        {
            var productCats = _context.ProductCategories.Where(pc => pc.ProductId == productId).ToList();

            foreach (var cat in productCats)
            {
                _context.ProductCategories.Remove(cat);
            }

            _context.SaveChanges();
        }

        public void UpdateProductCategory(int productId, string[] categories)
        {
            DeleteProductCategory(productId);
            AddCategoryToProduct(productId, categories);
        }

        public void AddTagsToProduct(string TagNames, int productId)
        {
            foreach (var Tag in TagNames.Split("-"))
            {
                var existTag = _context.Tags.Where(t => t.Name == Tag.Trim()).FirstOrDefault();

                if (existTag == null)
                {
                    Tag newTag = new Tag()
                    {
                        Name = Tag.Trim(),
                    };

                    _context.Tags.Add(newTag);
                    _context.SaveChanges();

                    existTag = newTag;
                }

                ProductTag pt = new ProductTag()
                {
                    ProductId = productId,
                    TagId = existTag.Id,
                };

                _context.ProductTags.Add(pt);
            }

            _context.SaveChanges();
        }

        public void DeleteProductTag(int productId)
        {
            var tags = _context.ProductTags.Where(p => p.ProductId == productId).ToList();

            foreach (var tag in tags)
            {
                _context.ProductTags.Remove(tag);
            }

            _context.SaveChanges();
        }


        public void UpdateProductTag(string TagNames, int productId)
        {
            DeleteProductTag(productId);
            AddTagsToProduct(TagNames, productId);
        }

        public List<ProductTag> GetProductTags(int productId)
        {
            return _context.ProductTags.Include(t => t.Tag).Where(pt => pt.ProductId == productId).ToList();
        }

        public List<Datalayer.Entities.Attribute> GetAttributeProduct(int productId)
        {
            return _context.Attributes.Where(pa => pa.ProductId == productId).ToList();
        }

        public List<string> GetProductCategories(int productId)
        {
            return _context.ProductCategories.Where(pc => pc.ProductId == productId).Select(x => x.CategoryId.ToString()).ToList();
        }

        public List<string> GetProductCategoriesTitle(int productId)
        {
            return _context.ProductCategories.Where(pc => pc.ProductId == productId).Select(x => x.Category.Title).ToList();
        }

        public List<Product> GetWonderProducts(int Number = 0)
        {
            if (Number != 0)
                return _context.Products.Include(p => p.Photo).Where(p => p.IsWonderProduct && p.IsDeleted == false && p.Status == "Publish" && p.WonderTime > DateTime.Now && !p.IsHidden).Take(Number).ToList();
            else
                return _context.Products.Include(p => p.Photo).Where(p => p.IsWonderProduct && p.IsDeleted == false && p.Status == "Publish" && p.WonderTime > DateTime.Now && !p.IsHidden).ToList();

        }

        public int GetUncompletedOrders()
        {
            return _context.Orders.Where(o => o.IsFinal == true && o.Status == "NoShipping").Count();
        }

        public IQueryable<Order> GetOrderNumberForCurrentMonth()
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;

            int ShamsiYear = pc.GetYear(dt);
            int ShamsiMonth = pc.GetMonth(dt);

            DateTime First = new DateTime(ShamsiYear, ShamsiMonth, 1, pc);
            DateTime Last = new DateTime(ShamsiYear, ShamsiMonth, 30, pc);

            var orders = _context.Orders.Where(o => o.PaymentDate > First && o.PaymentDate < Last && o.IsFinal == true);

            return orders;
        }

        public List<Shipping> GetAllShipping()
        {
            return _context.Shippings.OrderByDescending(p => p.Id).ToList();
        }

        public List<Shipping> GetSpecificShipping(int page, int countPerPage)
        {
            return _context.Shippings.OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public Shipping GetShipping(int id)
        {
            return _context.Shippings.FirstOrDefault(p => p.Id == id);
        }

        public void SaveShipping(Shipping shipping)
        {
            _context.Shippings.Add(shipping);
            _context.SaveChanges();
        }

        public void FinishWonder(int ProductId)
        {
            var product = GetProduct(ProductId);

            if (product.IsWonderProduct)
            {

                product.IsWonderProduct = false;
                product.WonderTime = null;

                foreach (var op in product.Variables)
                {
                    op.SalePrice = null;
                }

                _context.SaveChanges();
            }
        }

        public void DeleteCouponOrder(int orderId)
        {
            var order = GetOrderById(orderId);
            order.CouponId = null;
            order.CouponName = null;
            order.CouponOffValue = 0;
            order.OffPercent = 0;

            _context.SaveChanges();
        }

        public List<Brand> GetAllBrands()
        {
            return _context.Brands.ToList();
        }

        public List<Brand> GetSpecificBrands(int page, int countPerPage)
        {
            return _context.Brands.OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

        }

        public Brand GetBrand(int id)
        {
            return _context.Brands.FirstOrDefault(c => c.Id == id);
        }

        public Brand AddNewOrEditBrand(int BrandId, string BrandName, string BrandLink)
        {

            if (BrandId == 0)
            {
                var q = (from a in _context.Brands where a.Name == BrandName select a);

                if (q.Count() > 0)
                {
                    return null;
                }

                Brand brand;
                brand = new Brand()
                {
                    Name = BrandName,
                    Link = BrandLink
                };

                _context.Brands.Add(brand);
                _context.SaveChanges();
                return brand;
            }
            else
            {
                Brand brand = (from b in _context.Brands where b.Id == BrandId select b).FirstOrDefault();
                brand.Name = BrandName;
                brand.Link = BrandLink;

                _context.SaveChanges();
                return brand;
            }
        }

        public void DeleteBrand(int id)
        {
            var brand = GetBrand(id);

            _context.Brands.Remove(brand);
            _context.SaveChanges();
        }
    }
}
