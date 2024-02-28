using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class HospitalDAO
    {
        public static HospitalDAO instance;
        public static readonly object instanceLock = new object();
        public static HospitalDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new HospitalDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<Requests> GetRequestsByHospital(int id)
        {
            List<Requests> hospitals;
            try
            {
                var connectDB = new ConnectDB();
                hospitals = (from h in connectDB.Requests
                             join r in connectDB.Hospitals on h.Hospitalid equals r.Hospitalid
                             where h.Hospitalid == id
                             select new Requests
                             {
                                 Requestid = h.Requestid,
                                 Hospitalid = h.Hospitalid,
                                 RequestDate = h.RequestDate,
                                 quantity = h.quantity,
                                 Contact = h.Contact,
                                 Starttime = h.Starttime,
                                 Endtime = h.Endtime,
                                 City = h.City,
                                 Ward = h.Ward,
                                 District = h.District,
                                 Address = h.Address,
                                 Hospitals = new Hospitals
                                 {
                                     Hospitalid = r.Hospitalid,
                                     NameHospital = r.NameHospital,
                                     Users = r.Users,
                                 }
                             }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return hospitals;
        }

        public void AddRequest(Requests request)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Requests.Add(request);
                connectDB.SaveChanges();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void UpdateRequest(Requests request)
        {
            try
            {
                var connectDB = new ConnectDB();
                var r = connectDB.Requests.FirstOrDefault(n => n.Requestid == request.Requestid);
                if (r != null)
                {
                    r.RequestDate = request.RequestDate;
                    r.quantity = request.quantity;
                    r.Contact = request.Contact;
                    r.Starttime = request.Starttime;
                    r.Endtime = request.Endtime;
                    r.City = request.City;
                    r.Ward = request.Ward;
                    r.District = request.District;
                    r.Address = request.Address;
                    connectDB.Entry(r).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Request not found", nameof(r.Requestid));
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void DeleteRequest(int id)
        {
            try
            {
                var connectDB = new ConnectDB();
                var d = connectDB.Requests.FirstOrDefault(r => r.Requestid == id);
                if (d != null)
                {
                    connectDB.Requests.Remove(d);
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Request not found", nameof(id));
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public Requests listvolunteerregister(int id)
        {
            Requests request;
            try
            {
                var connectDB = new ConnectDB();
                request = connectDB.Requests.Where(r => r.Requestid == id).Select(c => new Requests
                {
                    Requestid = c.Requestid,
                    RequestDate = c.RequestDate,
                    quantity = c.quantity,
                    Contact = c.Contact,
                    Starttime = c.Starttime,
                    Endtime = c.Endtime,
                    City = c.City,
                    Ward = c.Ward,
                    District = c.District,
                    Address = c.Address,
                    Hospitals = connectDB.Hospitals.FirstOrDefault(h => h.Hospitalid == c.Hospitalid),
                    Registers = connectDB.Registers.Where(e => e.Requestid == c.Requestid).Select(b => new Registers
                    {
                        RegisterId = b.RegisterId,
                        Quantity = b.Quantity,
                        Volunteers = connectDB.Volunteers.Where(u => u.Volunteerid == b.Volunteerid).Select(p => new Volunteers
                        {
                            Volunteerid= p.Volunteerid,
                            Birthdate= p.Birthdate,
                            CCCD =p.CCCD,
                            Fullname= p.Fullname,
                            Gender= p.Gender,
                            Users = connectDB.Users.FirstOrDefault(m=>m.UserId == p.Volunteerid)
                        }).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.FirstOrDefault(z => z.Bloodtypeid == b.Bloodtypeid)
                    }).ToList()
                }).FirstOrDefault();
            }catch(Exception ex) { throw new Exception(ex.Message); }
            return request;
        }
        public Requests listhadvolunteerregister(int id)
        {
            Requests request;
            try
            {
                var connectDB = new ConnectDB();
                request = (from c in connectDB.Requests
                           join r in connectDB.Registers on c.Requestid equals r.Requestid
                           where c.Requestid == id && r.Quantity != 0
                           select new Requests
                           {
                               Requestid = c.Requestid,
                               RequestDate = c.RequestDate,
                               quantity = c.quantity,
                               Contact = c.Contact,
                               Starttime = c.Starttime,
                               Endtime = c.Endtime,
                               City = c.City,
                               Ward = c.Ward,
                               District = c.District,
                               Address = c.Address,
                               Hospitals = connectDB.Hospitals.FirstOrDefault(h => h.Hospitalid == c.Hospitalid),
                               Registers = connectDB.Registers.Where(e => e.Requestid == c.Requestid).Select(b => new Registers
                               {
                                   RegisterId = b.RegisterId,
                                   Quantity = b.Quantity,
                                   Volunteers = connectDB.Volunteers.Where(u => u.Volunteerid == b.Volunteerid).Select(p => new Volunteers
                                   {
                                       Volunteerid = p.Volunteerid,
                                       Birthdate = p.Birthdate,
                                       CCCD = p.CCCD,
                                       Fullname = p.Fullname,
                                       Gender = p.Gender,
                                       Users = connectDB.Users.FirstOrDefault(m => m.UserId == p.Volunteerid)
                                   }).FirstOrDefault(),
                                   Bloodtypes = connectDB.Bloodtypes.FirstOrDefault(z => z.Bloodtypeid == b.Bloodtypeid)
                               }).ToList()
                           }).FirstOrDefault();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return request;
        }
        public void UpdateRegister(Registers registers)
        {
            try
            {
                var connectDB = new ConnectDB();
                var r = connectDB.Registers.FirstOrDefault(n => n.RegisterId == registers.RegisterId);
                if (r != null)
                {
                    r.Quantity = registers.Quantity;
                    r.Bloodtypeid = registers.Bloodtypeid;
                    connectDB.Entry(r).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Regiter not found", nameof(r.Requestid));
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public IEnumerable<Bloodtypes> GetBloodtypes()
        {
            List<Bloodtypes> bloodtypes;
            try
            {
                var connectDB = new ConnectDB();
                bloodtypes = connectDB.Bloodtypes.ToList();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return bloodtypes;
        }
        
    }
}
