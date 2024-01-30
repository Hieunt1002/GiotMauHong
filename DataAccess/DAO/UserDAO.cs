using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Users
                        where u.Email == email && u.Password == password
                        select new Users
                        {
                            UserId = u.UserId,
                            Email = u.Email,
                            Password = u.Password,
                            Role = u.Role
                        }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public Users GetDefaultMember(string email, string password)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            string _email = configuration["account:admin:email"];
            string _password = configuration["account:admin:password"];

            if (_email.Equals(email) && _password.Equals(password))
            {
                return new Users
                {
                    UserId = 0,
                    Role = 0,
                    Email = _email,
                    Password = _password
                };
            }
            else
            {
                return null;
            }
        }
        public Users GetUserByEmail(string email)
        {
            Users user = null;
            try
            {
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Users
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
        public Users getUserid (string email)
        {

            Users user = null;
            try
            {
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Users
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
        public void AddVolunteers(Volunteers volunteers)
        {
            try
            {
               var connectDB = new ConnectDB();
               connectDB.Volunteers.Add(volunteers);
               connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddUser(Users user)
        {
            try
            {
                var mail = GetUserByEmail(user.Email);
                if (mail == null)
                {
                    var connectDB = new ConnectDB();
                    connectDB.Users.Add(user);
                    connectDB.SaveChanges();
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
        public Users checkpass(string email, string pass)
        {
            Users user = null;
            try
            {
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Users
                        where u.Email == email && u.Password == pass
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
        public void ChangePass(Users users)
        {
            var id = getUserid(users.Email);
            if (id != null)
            {
                var connectDB = new ConnectDB();
                connectDB.Entry<Users>(users).State = EntityState.Modified;
                connectDB.SaveChanges();
            }
            else
            {
                throw new Exception("The User does not already exist.");
            }
        }
    }
}
