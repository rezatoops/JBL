using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = Datalayer.Entities.Attribute;

namespace Core.Services.Interfaces
{
    public interface IProductService
    {
        string GetUnicSlug(string slug, int id);

        void SaveProduct(Product product);

        public List<Product> GetAllProduct(int number = -1);
        public List<Product> GetAllProduct(string search);
        List<Product> GetSpecificProduct(int page, int countPerPage,string? search);

        public Product GetProduct(int id);
        public Product GetProduct(string slug);

        public List<Media> GetGalleryPhotos(string Photos);

        public Variable? GetSelectedVariable(int variableId);

        public void AddVariableProductToOrder(int VariableId, int OrderId,int number);
        public void AddCustomVariableProductToOrder(OrderProduct op);

        public bool isVarProductExistInOrder(int VariableId, int OrderId);

        public List<OrderProduct> GetProductsInOrder(int orderId);

        public int GetTotalPriceInOrder(int OrderId);

        public int GetTotalRegularPriceInOrder(int OrderId);

        public int GetTotalPriceWhenVerify(int OrderId);
        public int GetTotalNumberInOrder(int OrderId);

        public void DeleteProductFromOrder(int OrderProductId);

        public void DeleteEmptyOrder(int orderId);

        public OrderProduct ChangeProductNumberInCart(int OrderProductId, int number);

        public OrderProduct GetOrderProductById(int OrderProductId);

        public Order GetOrderById(int OrderId);

        public List<Shipping> GetAllShippingMethods();

        //public int GetShippingPrice(int orderId);

        public void SetShippingPrice(Order order, int shippMethodId);

        public List<City> getStates();

        public List<City> getCityOfState(int StateId);

        public int GetStateOfCity(int cityId);

        public void SetCheckout(Order order, string phoneNumber, string UserDeliverName, string UserDeliverKodeMelli, string UserPhoneNumber, string UserHomePhone, int City, string Address, string PostalCode,string Description);

        public string VerifyProductCountInCart(OrderProduct OrderProdcut);

        public void PrepareOrderBeforePayment(Order order);

        public void RefundOrderAfterFailedPayment(Order order);
        public void CloseOrder(Order order, string paymentInfo);

        public List<Order> GetAllOrders();

        public List<Order> GetSpecificOrders(int page, int countPerPage);

        public City GetCityById(int cityId);
        public void SaveDatabse();

        public void SetStatusSelected(Order order, string status, string TrackingCode);

        public void DeleteOrder(int id);

        public void DeleteProduct(int Id);

        public List<Coupon> GetSpecificCouponCodes(int page, int countPerPage);

        public List<Coupon> GetAllCoupons();

        public Coupon GetCoupon(int CouponId);

        public Coupon? IsExistCoupon(string code);
        public bool IsUserUsedCoupon(int userId, int couponId);

        public int AddCouponCode(string Code, int CodePercent, int? MaxValue, DateTime? StartDate, DateTime? EndDate, int? MinTotal, bool onlyFirstOrder, bool AnyUserOneTime);
        public void UpdateCouponCode(int CouponId, int CodePercent, int? MaxValue, DateTime? StartDate, DateTime? EndDate, int? MinTotal, bool onlyFirstOrder, bool AnyUserOneTime);

        public int GetTotalCouponUsed(int CouponId);

        public void DeleteCoupon(int CouponId);

        public string VerifyCouponInCart(int orderId);

        public List<Product> GetAllPublishedProduct(int number = -1, bool IsOnlyAvailable = false);

        public List<Product> GetAllPublishedProduct(string search);

        public int CalculateOffValue(Order order, int TotalPrice);

        //public string SubmitCoupon(int OrderId, string CouponCode);

        //public void SendNitifSms(int productId);

        public Variable GetMainVariable(int productId);
        public string GetPriceText(Variable? variable);
        public void AddVariableToProduct(int productId, string[] title, string[] description, string[] price, string[] salePrice, string[] neumberInStock);
        public void DeleteProductVariables(int productId);
        public List<Variable> GetProductVariables(int productId);
        public void UpdateProductVariables(int productId, string[] title, string[] description, string[] price, string[] salePrice, string[] neumberInStock);
        public void AddAttributeToProduct(int productId, string[] selectedAttributes, string[] attributeValues);
        public void DeleteProductAttributes(int productId);
        public void UpdateProductAttributes(int productId, string[] selectedAttributes, string[] attributeValues);
        public List<Attribute> GetProductAttributes(int productId);
        public void AddCategoryToProduct(int productId, string[] categories);
        public void DeleteProductCategory(int productId);
        public void UpdateProductCategory(int productId, string[] categories);
        public void AddTagsToProduct(string TagNames, int productId);
        public void DeleteProductTag(int productId);
        public void UpdateProductTag(string TagNames, int productId);
        public List<ProductTag> GetProductTags(int productId);
        public List<Datalayer.Entities.Attribute> GetAttributeProduct(int productId);

        public List<string> GetProductCategories(int productId);
        public List<string> GetProductCategoriesTitle(int productId);

        public List<Product> GetWonderProducts(int Number = 4);

        public int GetUncompletedOrders();
        public IQueryable<Order> GetOrderNumberForCurrentMonth();
        public List<Shipping> GetAllShipping();
        public List<Shipping> GetSpecificShipping(int page, int countPerPage);
        public Shipping GetShipping(int id);

        public void SaveShipping(Shipping shipping);

        public void FinishWonder(int ProductId);

        public void DeleteCouponOrder(int orderId);

        public List<Brand> GetAllBrands();

        public Brand GetBrand(int id);

        public Brand AddNewOrEditBrand(int BrandId, string BrandName, string BrandLink);
        public void DeleteBrand(int id);

        public List<Brand> GetSpecificBrands(int page, int countPerPage);
    }
}
