using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AdminDAO
    {
        public static AdminDAO instance;
        public static readonly object instanceLock = new object();
        public static AdminDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AdminDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Activate> GetActivates()
        {
            List<Activate> activates;
            try
            {
                var connectDB = new ConnectDB();
                activates = connectDB.Activate.Select(a => new Activate
                {
                    Activateid= a.Activateid,
                    NameActivate = a.NameActivate,
                    Datepost = a.Datepost,
                    Images = connectDB.Images.Where(i => i.Activateid == a.Activateid).ToList(),
                }).ToList();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return activates;
        }
        public Activate GetActivatesbyID(int id)
        {
            Activate activates;
            try
            {
                var connectDB = new ConnectDB();
                activates = connectDB.Activate.Select(a => new Activate
                {
                    Activateid = a.Activateid,
                    NameActivate = a.NameActivate,
                    Datepost = a.Datepost,
                    Images = connectDB.Images.Where(i => i.Activateid == a.Activateid).ToList(),
                }).FirstOrDefault();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return activates;
        }
        public void AddActive(Activate activate)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Activate.Add(activate);
                connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void AddImgforActive(Images images)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Images.Add(images);
                connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void UpdateImgforActive(Images images)
        {
            try
            {
                var connectDB = new ConnectDB();
                var r = connectDB.Images.FirstOrDefault(n => n.ImgId == images.ImgId);
                if (r != null)
                {
                    r.Img = images.Img;
                    connectDB.Entry(r).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void UpdateActive(Activate activate)
        {
            try
            {
                var connectDB = new ConnectDB();
                var r = connectDB.Activate.FirstOrDefault(n => n.Activateid == activate.Activateid);
                if (r != null)
                {
                    r.NameActivate = activate.NameActivate;
                    connectDB.Entry(r).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public void DeleteActive(int id)
        {
            try
            {
                using (var connectDB = new ConnectDB())
                {
                    var d = connectDB.Activate.FirstOrDefault(r => r.Activateid == id);
                    var i = connectDB.Images.FirstOrDefault(r => r.Activateid == id);
                    if (d != null)
                    {
                        connectDB.Activate.Remove(d);
                        if (i != null)
                        {
                            connectDB.Images.Remove(i);
                        }
                        connectDB.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException("Request not found", nameof(id));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        public void AddBloodBank(Bloodbank bloodbank)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Bloodbank.Add(bloodbank);
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
        public int GetActive()
        {
            int id = 0;
            try
            {
                using (var connectDB = new ConnectDB())
                {
                    id = connectDB.Activate
                        .Where(c => !connectDB.Images.Any(i => i.Activateid == c.Activateid))
                        .Select(c => c.Activateid)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching active ID: " + ex.Message, ex);
            }
            return id;
        }
        public IEnumerable<Users> GetListBloodbank()
        {
            List<Users> hospitals;
            try
            {
                var connectDB = new ConnectDB();
                hospitals = (from u in connectDB.Users
                             join h in connectDB.Bloodbank on u.UserId equals h.Bloodbankid
                             select new Users
                             {
                                 UserId = u.UserId,
                                 Img = u.Img,
                                 Email = u.Email,
                                 PhoneNumber = u.PhoneNumber,
                                 City = u.City,
                                 Ward = u.Ward,
                                 District = u.District,
                                 Address = u.Address,
                                 Bloodbank = h,
                                 deactive = u.deactive
                             }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return hospitals;
        }

    }
}
