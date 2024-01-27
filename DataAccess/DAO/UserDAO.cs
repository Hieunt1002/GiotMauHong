using BusinessObject.Context;
using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class UserDAO
    {
        public static UserDAO instance;
        public static readonly object instanceLock = new object();
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }
        public Users Login(string email, string password)
        {
            Users user = null;
            try
            {
                var shopContext = new ConnectDB();
                user = (from u in shopContext.Users
                        where u.Email == email && u.Password == password
                        select new Users
                        {
                            UserId = u.UserId,
                            Email = u.Email,
                            Password = u.Password
                        }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public Users GetUserByEmail(string email)
        {
            Users user = null;
            try
            {
                var shopContext = new ConnectDB();
                user = (from u in shopContext.Users
                        where u.Email == email
                        select new Users
                        {
                            UserId = u.UserId,
                            Email = u.Email,
                            Role = u.Role
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public void AddUser(Users user)
        {
            try
            {
                var mail = GetUserByEmail(user.Email);
                if (mail == null)
                {
                    var shopContext = new ConnectDB();
                    shopContext.Users.Add(user);
                    shopContext.SaveChanges();
                }
                else
                {
                    throw new Exception("The email already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
