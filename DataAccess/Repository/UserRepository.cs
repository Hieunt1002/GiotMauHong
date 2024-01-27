using BusinessObject.Model;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(Users user) => UserDAO.Instance.AddUser(user);

        public Users Login(string email, string password) => UserDAO.Instance.Login(email, password);
    }
}
