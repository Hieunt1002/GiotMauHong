using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
                                 img = h.img,
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
                                 },
                                 status = h.status
                             }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return hospitals;
        }
        public Requests GetRequestsByid(int id)
        {
            Requests hospitals;
            try
            {
                var connectDB = new ConnectDB();
                hospitals = (from h in connectDB.Requests
                             join r in connectDB.Hospitals on h.Hospitalid equals r.Hospitalid
                             where h.Requestid == id
                             select new Requests
                             {
                                 Requestid = h.Requestid,
                                 Hospitalid = h.Hospitalid,
                                 img = h.img,
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
                                 },
                                 status = h.status
                             }).FirstOrDefault();
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
                    r.img = request.img;
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
                var d = connectDB.Requests.FirstOrDefault(r => r.Requestid == id && r.RequestDate > DateTime.Now);
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
                    img = c.img,
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
                    }).ToList(),
                    status = c.status
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
                               img = c.img,
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
                               }).ToList(),
                               status = c.status
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
        public void AddSendBlood(SendBlood sendBlood)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.SendBlood.Add(sendBlood);
                connectDB.SaveChanges();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void AddTakeBlood(Takebloods takebloods)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.Takebloods.Add(takebloods);
                connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void QuantityTakeBlood(QuantityTake quantityTake)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.QuantityTake.Add(quantityTake);
                connectDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int takebookid(int hopitalid)
        {
            int id = 0;
            try
            {
                var connectDB = new ConnectDB();
                id = (from c in connectDB.Takebloods
                      where c.Hospitalid == hopitalid && c.Status == 0
                      select c.Takebloodid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }
        public int sendbookid(int hopitalid)
        {
            int id = 0;
            try
            {
                var connectDB = new ConnectDB();
                id = (from c in connectDB.SendBlood
                      where c.Hospitalid == hopitalid && c.Status == 0
                      select c.SendBloodid).FirstOrDefault();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }
        public IEnumerable<NumberBlood> listnumberBloods()
        {
            List<NumberBlood> numberBloods;
            try
            {
                var connectDB = new ConnectDB();
                numberBloods = connectDB.NumberBlood.ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return numberBloods;
        }
        public void QuantitySendBlood(QuantitySend quantitySend)
        {
            try
            {
                var connectDB = new ConnectDB();
                connectDB.QuantitySend.Add(quantitySend);
                connectDB.SaveChanges();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void updatesendblood(QuantitySend sendBlood)
        {
            try
            {
                var connectDB = new ConnectDB();
                var s = connectDB.QuantitySend.FirstOrDefault(C => C.quantitysendid == sendBlood.quantitysendid);
                if (s != null)
                {
                    s.numberbloodid = sendBlood.numberbloodid;
                    s.Bloodtypeid = sendBlood.Bloodtypeid;
                    s.quantity = sendBlood.quantity;
                    connectDB.Entry(s).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Regiter not found", nameof(s.SendBloodid));
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void updatetakeblood(QuantityTake quantityTake)
        {
            try
            {
                var connectDB = new ConnectDB();
                var s = connectDB.QuantityTake.FirstOrDefault(C => C.quantitytakeid == quantityTake.quantitytakeid);
                if (s != null)
                {
                    s.numberbloodid = quantityTake.numberbloodid;
                    s.Bloodtypeid = quantityTake.Bloodtypeid;
                    s.quantity = quantityTake.quantity;
                    connectDB.Entry(s).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Regiter not found", nameof(s.Takebloodid));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void cancelsendblood(int id)
        {
            try
            {
                var connectDB = new ConnectDB();
                var s = connectDB.SendBlood.FirstOrDefault(C => C.SendBloodid == id);
                var a = connectDB.QuantitySend.FirstOrDefault(C => C.SendBloodid == id);
                if (s != null)
                {
                    connectDB.SendBlood.Remove(s);
                    if(a != null)
                    {
                        connectDB.QuantitySend.Remove(a);
                    }
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Regiter not found", nameof(s.SendBloodid));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void canceltakeblood(int id)
        {
            try
            {
                var connectDB = new ConnectDB();
                var s = connectDB.Takebloods.FirstOrDefault(C => C.Takebloodid == id);
                var a = connectDB.QuantityTake.FirstOrDefault(C => C.Takebloodid == id);
                if (s != null)
                {
                    connectDB.Takebloods.Remove(s);
                    if (a != null)
                    {
                        connectDB.QuantityTake.Remove(a);
                    }
                    connectDB.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Regiter not found", nameof(s.Takebloodid));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<SendBlood> GetSendBlood(int id)
        {
            List<SendBlood> sendBloods;
            try
            {
                var connectDB = new ConnectDB();
                sendBloods = connectDB.SendBlood.Where(i => i.Hospitalid == id).Select(s => new SendBlood
                {
                    SendBloodid = s.SendBloodid,
                    Hospitalid = s.Hospitalid,
                    Bloodbankid = s.Bloodbankid,
                    Datesend = s.Datesend,
                    Status = s.Status,
                    Hospitals = connectDB.Hospitals.Where(h => h.Hospitalid == s.Hospitalid).Select(h => new Hospitals
                    {
                        Hospitalid = h.Hospitalid,
                        NameHospital = h.NameHospital,
                        Users = connectDB.Users.Where(u => u.UserId == h.Hospitalid).FirstOrDefault()
                    }).FirstOrDefault(),
                    Bloodbank = connectDB.Bloodbank.Where(h => h.Bloodbankid == s.Bloodbankid).Select(h => new Bloodbank
                    {
                        Bloodbankid = h.Bloodbankid,
                        NameBloodbank = h.NameBloodbank,
                        Users = connectDB.Users.Where(u => u.UserId == h.Bloodbankid).FirstOrDefault()
                    }).FirstOrDefault(),
                    QuantitySends = connectDB.QuantitySend.Where(q => q.SendBloodid == s.SendBloodid).Select(a => new QuantitySend
                    {
                        quantitysendid = a.quantitysendid,
                        numberbloodid = a.numberbloodid,
                        SendBloodid = a.SendBloodid,
                        Bloodtypeid = a.Bloodtypeid,
                        quantity = a.quantity,
                        NumberBlood = connectDB.NumberBlood.Where(n => n.numberbloodid == a.numberbloodid).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.Where(b => b.Bloodtypeid == a.Bloodtypeid).FirstOrDefault()
                    }).ToList()
                }).ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sendBloods;
        }
        public IEnumerable<SendBlood> GetSendBloodbyhospitalid(int id)
        {
            List<SendBlood> sendBloods;
            try
            {
                var connectDB = new ConnectDB();
                sendBloods = connectDB.SendBlood.Where(a => a.Hospitalid == id).Select(s => new SendBlood
                {
                    SendBloodid = s.SendBloodid,
                    Hospitalid = s.Hospitalid,
                    Bloodbankid = s.Bloodbankid,
                    Datesend = s.Datesend,
                    Status = s.Status,
                    Hospitals = connectDB.Hospitals.Where(h => h.Hospitalid == s.Hospitalid).Select(h => new Hospitals
                    {
                        Hospitalid = h.Hospitalid,
                        NameHospital = h.NameHospital,
                        Users = connectDB.Users.Where(u => u.UserId == h.Hospitalid).FirstOrDefault()
                    }).FirstOrDefault(),
                    Bloodbank = connectDB.Bloodbank.Where(h => h.Bloodbankid == s.Bloodbankid).Select(h => new Bloodbank
                    {
                        Bloodbankid = h.Bloodbankid,
                        NameBloodbank = h.NameBloodbank,
                        Users = connectDB.Users.Where(u => u.UserId == h.Bloodbankid).FirstOrDefault()
                    }).FirstOrDefault(),
                    QuantitySends = connectDB.QuantitySend.Where(q => q.SendBloodid == s.SendBloodid).Select(a => new QuantitySend
                    {
                        quantitysendid = a.quantitysendid,
                        numberbloodid = a.numberbloodid,
                        SendBloodid = a.SendBloodid,
                        Bloodtypeid = a.Bloodtypeid,
                        quantity = a.quantity,
                        NumberBlood = connectDB.NumberBlood.Where(n => n.numberbloodid == a.numberbloodid).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.Where(b => b.Bloodtypeid == a.Bloodtypeid).FirstOrDefault()
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sendBloods;
        }
        public SendBlood GetSendBloodbyid(int id)
        {
            SendBlood sendBloods;
            try
            {
                var connectDB = new ConnectDB();
                sendBloods = connectDB.SendBlood.Where(i => i.SendBloodid == id).Select(s => new SendBlood
                {
                    SendBloodid = s.SendBloodid,
                    Hospitalid = s.Hospitalid,
                    Bloodbankid = s.Bloodbankid,
                    Datesend = s.Datesend,
                    Status = s.Status,
                    Hospitals = connectDB.Hospitals.Where(h => h.Hospitalid == s.Hospitalid).Select(h => new Hospitals
                    {
                        Hospitalid = h.Hospitalid,
                        NameHospital = h.NameHospital,
                        Users = connectDB.Users.Where(u => u.UserId == h.Hospitalid).FirstOrDefault()
                    }).FirstOrDefault(),
                    Bloodbank = connectDB.Bloodbank.Where(h => h.Bloodbankid == s.Bloodbankid).Select(h => new Bloodbank
                    {
                        Bloodbankid = h.Bloodbankid,
                        NameBloodbank = h.NameBloodbank,
                        Users = connectDB.Users.Where(u => u.UserId == h.Bloodbankid).FirstOrDefault()
                    }).FirstOrDefault(),
                    QuantitySends = connectDB.QuantitySend.Where(q => q.SendBloodid == s.SendBloodid).Select(a => new QuantitySend
                    {
                        quantitysendid = a.quantitysendid,
                        numberbloodid = a.numberbloodid,
                        SendBloodid = a.SendBloodid,
                        Bloodtypeid = a.Bloodtypeid,
                        quantity = a.quantity,
                        NumberBlood = connectDB.NumberBlood.Where(n => n.numberbloodid == a.numberbloodid).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.Where(b => b.Bloodtypeid == a.Bloodtypeid).FirstOrDefault()
                    }).ToList()
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sendBloods;
        }
        public IEnumerable<Takebloods> GetTakeBlood(int id)
        {
            List<Takebloods> sendBloods;
            try
            {
                var connectDB = new ConnectDB();
                sendBloods = connectDB.Takebloods.Where(i => i.Hospitalid == id).Select(s => new Takebloods
                {
                    Takebloodid = s.Takebloodid,
                    Hospitalid = s.Hospitalid,
                    Bloodbankid = s.Bloodbankid,
                    Datetake = s.Datetake,
                    Status = s.Status,
                    Hospitals = connectDB.Hospitals.Where(h => h.Hospitalid == s.Hospitalid).Select(h => new Hospitals
                    {
                        Hospitalid = h.Hospitalid,
                        NameHospital = h.NameHospital,
                        Users = connectDB.Users.Where(u => u.UserId == h.Hospitalid).FirstOrDefault()
                    }).FirstOrDefault(),
                    Bloodbank = connectDB.Bloodbank.Where(h => h.Bloodbankid == s.Bloodbankid).Select(h => new Bloodbank
                    {
                        Bloodbankid = h.Bloodbankid,
                        NameBloodbank = h.NameBloodbank,
                        Users = connectDB.Users.Where(u => u.UserId == h.Bloodbankid).FirstOrDefault()
                    }).FirstOrDefault(),
                    QuantityTake = connectDB.QuantityTake.Where(q => q.quantitytakeid == s.Takebloodid).Select(a => new QuantityTake
                    {
                        quantitytakeid = a.quantitytakeid,
                        numberbloodid = a.numberbloodid,
                        Takebloodid = a.Takebloodid,
                        Bloodtypeid = a.Bloodtypeid,
                        quantity = a.quantity,
                        NumberBlood = connectDB.NumberBlood.Where(n => n.numberbloodid == a.numberbloodid).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.Where(b => b.Bloodtypeid == a.Bloodtypeid).FirstOrDefault()
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sendBloods;
        }
        public IEnumerable<Takebloods> GetTakeBloodbyid(int id)
        {
            List<Takebloods> sendBloods;
            try
            {
                var connectDB = new ConnectDB();
                sendBloods = connectDB.Takebloods.Where(a => a.Takebloodid == id).Select(s => new Takebloods
                {
                    Takebloodid = s.Takebloodid,
                    Hospitalid = s.Hospitalid,
                    Bloodbankid = s.Bloodbankid,
                    Datetake = s.Datetake,
                    Status = s.Status,
                    Hospitals = connectDB.Hospitals.Where(h => h.Hospitalid == s.Hospitalid).Select(h => new Hospitals
                    {
                        Hospitalid = h.Hospitalid,
                        NameHospital = h.NameHospital,
                        Users = connectDB.Users.Where(u => u.UserId == h.Hospitalid).FirstOrDefault()
                    }).FirstOrDefault(),
                    Bloodbank = connectDB.Bloodbank.Where(h => h.Bloodbankid == s.Bloodbankid).Select(h => new Bloodbank
                    {
                        Bloodbankid = h.Bloodbankid,
                        NameBloodbank = h.NameBloodbank,
                        Users = connectDB.Users.Where(u => u.UserId == h.Bloodbankid).FirstOrDefault()
                    }).FirstOrDefault(),
                    QuantityTake = connectDB.QuantityTake.Where(q => q.quantitytakeid == s.Takebloodid).Select(a => new QuantityTake
                    {
                        quantitytakeid = a.quantitytakeid,
                        numberbloodid = a.numberbloodid,
                        Takebloodid = a.Takebloodid,
                        Bloodtypeid = a.Bloodtypeid,
                        quantity = a.quantity,
                        NumberBlood = connectDB.NumberBlood.Where(n => n.numberbloodid == a.numberbloodid).FirstOrDefault(),
                        Bloodtypes = connectDB.Bloodtypes.Where(b => b.Bloodtypeid == a.Bloodtypeid).FirstOrDefault()
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sendBloods;
        }
        public IEnumerable<NumberBloodDTO> listnumberblood(int id)
        {
            List<NumberBloodDTO> numberBloods;
            try
            {
                var connectDB = new ConnectDB();
                numberBloods = (from b in connectDB.Bloodtypes
                                where b.Bloodtypeid != 1
                                select new NumberBloodDTO
                                {
                                    Bloodtypeid = b.Bloodtypeid,
                                    NameBlood = b.NameBlood,
                                    totalBloodDTOs = (from nb in connectDB.NumberBlood
                                                      join qs in connectDB.QuantitySend on nb.numberbloodid equals qs.numberbloodid
                                                      join c in connectDB.SendBlood on qs.SendBloodid equals c.SendBloodid
                                                      where qs.Bloodtypeid == b.Bloodtypeid && c.Hospitalid == id
                                                      select new TotalBloodDTO
                                                      {
                                                          numberbloodid = nb.numberbloodid,
                                                          quantity = nb.quantity,
                                                          total = connectDB.QuantitySend.Where(s => s.Bloodtypeid == b.Bloodtypeid && s.numberbloodid == nb.numberbloodid).Sum(t => t.quantity) - 
                                                          connectDB.QuantityTake.Where(s => s.Bloodtypeid == b.Bloodtypeid && s.numberbloodid == nb.numberbloodid).Sum(t => t.quantity)
                                                      }).ToList()
                                }).ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return numberBloods;
        }
        public void updateProfileHospotal(Hospitals users)
        {
            try
            {
                var connectDB = new ConnectDB();
                var use = connectDB.Hospitals.FirstOrDefault(n => n.Hospitalid == users.Hospitalid);
                if (use != null)
                {
                    use.NameHospital = users.NameHospital;
                    connectDB.Entry(use).State = EntityState.Modified;
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
