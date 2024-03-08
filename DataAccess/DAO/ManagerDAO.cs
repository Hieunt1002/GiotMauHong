using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public IEnumerable<InforHospitalDTO> GetInforHospitalDTOs(int id)
        {
            List<InforHospitalDTO> inforHospitalDTOs;
            try
            {
                var connectDB = new ConnectDB();
                inforHospitalDTOs = (from u in connectDB.Users
                                     join h in connectDB.Hospitals on u.UserId equals h.Hospitalid
                                     join q in connectDB.SendBlood on h.Hospitalid equals q.Hospitalid
                                     where q.Bloodbankid == id
                                     select new InforHospitalDTO
                                     {
                                         UserId = u.UserId,
                                         Img = u.Img,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         City = u.City,
                                         Ward = u.Ward,
                                         District = u.District,
                                         Address = u.Address,
                                         NameHospital = h.NameHospital,
                                         NumberBlood = (from qn in connectDB.QuantitySend
                                                        join b in connectDB.Bloodtypes on qn.Bloodtypeid equals b.Bloodtypeid
                                                        where b.Bloodtypeid != 1 && qn.SendBloodid == q.SendBloodid
                                                        select new NumberBloodDTO
                                                        {
                                                            Bloodtypeid = b.Bloodtypeid,
                                                            NameBlood = b.NameBlood,
                                                            totalBloodDTOs = (from nb in connectDB.NumberBlood
                                                                              join qs in connectDB.QuantitySend on nb.numberbloodid equals qs.numberbloodid
                                                                              where qs.Bloodtypeid == b.Bloodtypeid
                                                                              select new TotalBloodDTO
                                                                              {
                                                                                  numberbloodid = nb.numberbloodid,
                                                                                  quantity = nb.quantity,
                                                                                  total = qs.quantity
                                                                              }).ToList()
                                                        }).ToList(),
                                         status = q.Status
                                     }).ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return inforHospitalDTOs;
        }
        public IEnumerable<InforHospitalDTO> GetInforHospitalTakeBlood(int id)
        {
            List<InforHospitalDTO> inforHospitalDTOs;
            try
            {
                var connectDB = new ConnectDB();
                inforHospitalDTOs = (from u in connectDB.Users
                                     join h in connectDB.Hospitals on u.UserId equals h.Hospitalid
                                     join q in connectDB.Takebloods on h.Hospitalid equals q.Hospitalid
                                     where q.Bloodbankid == id
                                     select new InforHospitalDTO
                                     {
                                         UserId = u.UserId,
                                         Img = u.Img,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         City = u.City,
                                         Ward = u.Ward,
                                         District = u.District,
                                         Address = u.Address,
                                         NameHospital = h.NameHospital,
                                         NumberBlood = (from qn in connectDB.QuantityTake
                                                        join b in connectDB.Bloodtypes on qn.Bloodtypeid equals b.Bloodtypeid
                                                        where b.Bloodtypeid != 1 && qn.Takebloodid == q.Takebloodid
                                                        select new NumberBloodDTO
                                                        {
                                                            Bloodtypeid = b.Bloodtypeid,
                                                            NameBlood = b.NameBlood,
                                                            totalBloodDTOs = (from nb in connectDB.NumberBlood
                                                                              join qs in connectDB.QuantityTake on nb.numberbloodid equals qs.numberbloodid
                                                                              where qs.Bloodtypeid == b.Bloodtypeid
                                                                              select new TotalBloodDTO
                                                                              {
                                                                                  numberbloodid = nb.numberbloodid,
                                                                                  quantity = nb.quantity,
                                                                                  total = qs.quantity
                                                                              }).ToList()
                                                        }).ToList(),
                                         status = q.Status
                                     }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return inforHospitalDTOs;
        }
        public void acceptSend(SendBlood sendBlood)
        {
            try
            {
                var connectDB = new ConnectDB();
                var send = connectDB.SendBlood.FirstOrDefault(s => s.SendBloodid == sendBlood.SendBloodid);
                if (send != null)
                {
                    send.Status = sendBlood.Status;
                    connectDB.Entry(send).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void accepttake(Takebloods takebloods)
        {
            try
            {
                var connectDB = new ConnectDB();
                var take = connectDB.Takebloods.FirstOrDefault(s => s.Takebloodid == takebloods.Takebloodid);
                if (take != null)
                {
                    take.Status = takebloods.Status;
                    connectDB.Entry(take).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
