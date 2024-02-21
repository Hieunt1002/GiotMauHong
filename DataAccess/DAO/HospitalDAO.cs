using Azure.Core;
using BusinessObject.Context;
using BusinessObject.Model;
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
                                 Starttime= h.Starttime,
                                 Endtime= h.Endtime,
                                 City=h.City,
                                 Ward=h.Ward,
                                 District=h.District,
                                 Address=h.Address,
                                 Hospitals = new Hospitals
                                 {
                                     Hospitalid= r.Hospitalid,
                                     NameHospital= r.NameHospital,
                                     Users=r.Users,
                                 }
                             }).ToList();
            }catch(Exception ex)
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
    }
}
