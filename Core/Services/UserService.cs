using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService:IUserService
    {
        private ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public User? isExistUser(string phoneNumber)
        {
            return _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }

        public void UpdateUser(User updatedUser)
        {
            _context.SaveChanges();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool CheckLoginWithPassword(string phoneNumber, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Password == password);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public List<User> GetAllUsers(string search)
        {
            return _context.Users.Where(p=> p.FirstName.Contains(search) || p.LastName.Contains(search) || p.PhoneNumber.Contains(search)).OrderByDescending(p => p.Id).ToList();
        }
        public List<User> GetSpecificUsers(int page, int countPerPage)
        {
            return _context.Users.OrderBy(u => u.Role).ThenBy(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void DeleteUser(int id)
        {
            var user = GetUserById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User SetRole(int userId, int roleId)
        {
            var user = GetUserById(userId);
            user.Role = roleId;

            _context.SaveChanges();

            return user;
        }

        public int IsOpenOrder(string phoneNumber)
        {
            var user = isExistUser(phoneNumber);
            var order = _context.Orders.Where(o => o.UserId == user.Id && o.IsFinal == false).FirstOrDefault();

            if (order == null)
            {
                return -1;
            }
            else
            {
                return order.Id;
            }
        }

        public int AddNewOrderToUser(string phoneNumber)
        {
            var user = isExistUser(phoneNumber);
            Order newOrder = new Order()
            {
                IsFinal = false,
                PhoneNumber = phoneNumber,
                UserId = user.Id,
                CityId = 360,
                Status = "NoPayment",
                CreationDate = DateTime.Now,
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return newOrder.Id;
        }

        public List<Order> GetUserOrders(string phoneNumber)
        {
            var user = isExistUser(phoneNumber);
            return _context.Orders.Where(o => o.UserId == user.Id).OrderByDescending(o => o.Id).ToList();
        }

        public bool IsUserHaveFinalOrder(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId && o.IsFinal).Any();
        }

        public void ChangeProfile(User user, string firstName, string lastName, string Password)
        {
            user.FirstName = firstName;
            user.LastName = lastName;
            if (!string.IsNullOrEmpty(Password))
            {
                user.Password = Password;
            }

            _context.SaveChanges();
        }

    }
}
