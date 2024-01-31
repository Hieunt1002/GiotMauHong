using BusinessObject.Context;
using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ManagerDAO
    {
        public static ManagerDAO instance;
        public static readonly object instanceLock = new object();
        public static ManagerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ManagerDAO();
                    }
                    return instance;
                }
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
        public void AddHospital(Hospitals hospitals)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Hospitals.Add(hospitals);
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
        public IEnumerable<Users> GetListHospital()
        {
            List<Users> hospitals;
            try
            {
                var connectDB = new ConnectDB();
                hospitals = (from u in connectDB.Users
                             join h in connectDB.Hospitals on u.UserId equals h.Hospitalid
                             where u.Role == 2
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
                                 Hospitals = h
                             }).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return hospitals;
        }
        public Users getUserid(string email)
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
    }
}
