using Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService
    {
        public User? isExistUser(string phoneNumber);

        public void UpdateUser(User updatedUser);

        public void AddUser(User user);

        public bool CheckLoginWithPassword(string phoneNumber, string password);

        public List<User> GetAllUsers();

        public List<User> GetAllUsers(string search);

        public User GetUserById(int id);

        public List<User> GetSpecificUsers(int page, int countPerPage);

        public void DeleteUser(int id);

        public User SetRole(int userId, int roleId);


        public int IsOpenOrder(string phoneNumber);

        public int AddNewOrderToUser(string phoneNumber);

        public List<Order> GetUserOrders(string phoneNumber);

        public bool IsUserHaveFinalOrder(int userId);

        public void ChangeProfile(User user, string firstName, string lastName, string Password);
    }
}
