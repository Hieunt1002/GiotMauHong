using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
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
        public Users getUsersid(int id)
        {

            Users user = null;
            try
            {
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Users
                        where u.UserId == id
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
        public ViewHistory gethistory(int id)
        {

            ViewHistory user = null;
            try
            {
                var connectDB = new ConnectDB();
                user = (from v in connectDB.Users
                        join vs in connectDB.Volunteers on v.UserId equals vs.Volunteerid
                        join rs in connectDB.Registers on vs.Volunteerid equals rs.Volunteerid
                        join bt in connectDB.Bloodtypes on rs.Bloodtypeid equals bt.Bloodtypeid
                        where v.UserId == id
                        select new ViewHistory
                        {
                            UserId = v.UserId,
                            Img = v.Img,
                            Email = v.Email,
                            PhoneNumber = v.PhoneNumber,
                            City = v.City,
                            Ward = v.Ward,
                            District = v.District,
                            Address = v.Address,
                            Volunteers = new Volunteers
                            {
                                Volunteerid = vs.Volunteerid,
                                Birthdate = vs.Birthdate,
                                Gender = vs.Gender,
                                Fullname = vs.Fullname,
                                CCCD = vs.CCCD,
                                Registers = connectDB.Registers.Where(r => r.Volunteerid == vs.Volunteerid)
                                                                .Select(r => new Registers
                                                                {
                                                                    RegisterId = r.RegisterId,
                                                                    Volunteerid = r.Volunteerid,
                                                                    Requestid = r.Requestid,
                                                                    Quantity = r.Quantity,
                                                                    Bloodtypeid = r.Bloodtypeid,
                                                                    Bloodtypes = bt
                                                                }).ToList()
                            }
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public Users getProfile(int id)
        {
            Users users = null;
            try
            {
                var u = getUsersid(id);
                if (u.Role == 1)
                {
                    var connectDB = new ConnectDB();
                    users = (from v in connectDB.Users
                             join vs in connectDB.Volunteers on v.UserId equals vs.Volunteerid
                             where v.UserId == id
                             select new Users
                             {
                                 UserId = v.UserId,
                                 Img = v.Img,
                                 Email = v.Email,
                                 PhoneNumber = v.PhoneNumber,
                                 City = v.City,
                                 Ward = v.Ward,
                                 District = v.District,
                                 Address = v.Address,
                                 Volunteers = vs
                             }).FirstOrDefault();
                }else if(u.Role == 2)
                {
                    var connectDB = new ConnectDB();
                    users = (from v in connectDB.Users
                             join vs in connectDB.Hospitals on v.UserId equals vs.Hospitalid
                             where v.UserId == id
                             select new Users
                             {
                                 UserId = v.UserId,
                                 Img = v.Img,
                                 Email = v.Email,
                                 PhoneNumber = v.PhoneNumber,
                                 City = v.City,
                                 Ward = v.Ward,
                                 District = v.District,
                                 Address = v.Address,
                                 Hospitals = vs
                             }).FirstOrDefault();
                }
                else
                {
                    var connectDB = new ConnectDB();
                    users = (from v in connectDB.Users
                             join vs in connectDB.Bloodbank on v.UserId equals vs.Bloodbankid
                             where v.UserId == id
                             select new Users
                             {
                                 UserId = v.UserId,
                                 Img = v.Img,
                                 Email = v.Email,
                                 PhoneNumber = v.PhoneNumber,
                                 City = v.City,
                                 Ward = v.Ward,
                                 District = v.District,
                                 Address = v.Address,
                                 Bloodbank = vs
                             }).FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
            
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
