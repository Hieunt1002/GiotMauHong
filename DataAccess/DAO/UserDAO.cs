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
        public void addnotification(Notification notification)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Notification.Add(notification);
                connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering request", ex);
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
        public async Task<string> forgotpass(string email, string content)
        {
            string result = null;
            try
            {
                if (IsValidEmail(email))
                {
                    var connectDB = new ConnectDB();
                    var user = (from u in connectDB.Users
                                where u.Email == email
                                select new Users
                                {
                                    Email = u.Email
                                }).FirstOrDefault();

                    if (user != null)
                    {
                        await MailUtils.SendMailGoogleSmtp("nguyenanh0978638@gmail.com",
                              user.Email,
                              "Forgot Password",
                              content,
                              "nguyenanh0978638@gmail.com", "kxnutqxwydifngae");
                        result = "Email sent successfully.";
                    }
                    else
                    {
                        result = "User not found for email " + email;
                    }
                }
                else
                {
                    result = "Invalid email address " + email;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request", ex);
            }
            return result;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Users> getUserid(int id)
        {
            Users user = null;
            try
            {
                using (var connectDB = new ConnectDB())
                {
                    user = await (from u in connectDB.Users
                                  where u.UserId == id
                                  select new Users
                                  {
                                      UserId = u.UserId,
                                      Email = u.Email,
                                      Role = u.Role
                                  }).FirstOrDefaultAsync();
                }
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
                    users = connectDB.Users.Where(u => u.UserId == id).Select(v => new Users
                    {
                        UserId = v.UserId,
                        Img = v.Img,
                        Email = v.Email,
                        PhoneNumber = v.PhoneNumber,
                        City = v.City,
                        Ward = v.Ward,
                        District = v.District,
                        Address = v.Address,
                        Volunteers = connectDB.Volunteers.FirstOrDefault(f => f.Volunteerid == v.UserId)
                    }).FirstOrDefault();
                }
                else if (u.Role == 2)
                {
                    var connectDB = new ConnectDB();
                    users = connectDB.Users.Where(u => u.UserId == id).Select(v => new Users
                    {
                        UserId = v.UserId,
                        Img = v.Img,
                        Email = v.Email,
                        PhoneNumber = v.PhoneNumber,
                        City = v.City,
                        Ward = v.Ward,
                        District = v.District,
                        Address = v.Address,
                        Hospitals = connectDB.Hospitals.FirstOrDefault(f => f.Hospitalid == v.UserId)
                    }).FirstOrDefault();
                }
                else
                {
                    var connectDB = new ConnectDB();
                    users = connectDB.Users.Where(u => u.UserId == id).Select(v => new Users
                    {
                        UserId = v.UserId,
                        Img = v.Img,
                        Email = v.Email,
                        PhoneNumber = v.PhoneNumber,
                        City = v.City,
                        Ward = v.Ward,
                        District = v.District,
                        Address = v.Address,
                        Bloodbank = connectDB.Bloodbank.FirstOrDefault(f => f.Bloodbankid == v.UserId)
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
            var id = GetUserByEmail(users.Email);
            if (id != null)
            {
                using (var connectDB = new ConnectDB())
                {
                    var email = connectDB.Users.FirstOrDefault(e => e.Email == users.Email);
                    if (email != null)
                    {
                        email.Password= users.Password;
                        connectDB.Entry(email).State = EntityState.Modified;
                        connectDB.SaveChanges();
                    }
                }
            }
            else
            {
                throw new Exception("The User does not already exist.");
            }
        }

    }
}
