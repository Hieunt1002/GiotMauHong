using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class VolunteersDAO
    {
        public static VolunteersDAO instance;
        public static readonly object instanceLock = new object();
        public static VolunteersDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VolunteersDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<ViewRequest> searchRequest(DateTime startdate, DateTime enddate, int volunteerid)
        {
            List<ViewRequest> requests = null;
            try
            {
                var connectDB = new ConnectDB();
                var query = (from r in connectDB.Requests
                            join h in connectDB.Hospitals on r.Hospitalid equals h.Hospitalid
                            join d in connectDB.Registers on r.Requestid equals d.Requestid into registerGroup
                            from d in registerGroup.DefaultIfEmpty()
                            where (r.RequestDate >= startdate && r.RequestDate <= enddate) &&
                                  (volunteerid == 0 || r.City == connectDB.Users.FirstOrDefault(u => u.UserId == volunteerid).City) &&
                                  r.status == 1
                            select new ViewRequest
                            {
                                Requestid = r.Requestid,
                                Hospitalid = r.Hospitalid,
                                Img = r.img,
                                RequestDate = r.RequestDate,
                                quantity = r.quantity,
                                Contact = r.Contact,
                                Starttime = r.Starttime,
                                Endtime = r.Endtime,
                                City = r.City,
                                Ward = r.Ward,
                                District = r.District,
                                Address = r.Address,
                                Hospitals = h,
                                total = ((double)connectDB.Registers.Count(c => c.Requestid == r.Requestid) / r.quantity) * 100,
                                status = r.RequestDate < DateTime.Today ? 0 : 1
                            }).Distinct();

                requests = query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return requests;
        }

        public async Task regesterRequest(Registers registers)
        {
            if (registers == null)
            {
                throw new ArgumentNullException(nameof(registers), "registers parameter cannot be null");
            }

            try
            {
                using (var connectDB = new ConnectDB())
                {
                    connectDB.Registers.Add(registers);
                    connectDB.SaveChanges();
                }

                UserDAO user = new UserDAO();
                var u = await user.getUserid(registers.Volunteerid);
                if (IsValidEmail(u.Email))
                {
                    await MailUtils.SendMailGoogleSmtp("loclhde150242@fpt.edu.vn",
                          u.Email,
                          "Đăng ký hiến máu",
                          "Chúc mừng bạn đã đăng ký thành công. Vui lòng đến đúng ngày và thời gian để tham gia hiến máu.",
                          "loclhde150242@fpt.edu.vn", "wkjkparmndknolv");
                }
                else
                {
                    throw new ArgumentException("Invalid email address", nameof(u.Email));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering request", ex);
            }
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
        private int listvolunteerid(int volunteerid)
        {
            int status = 0;
            try
            {
                var connectDB = new ConnectDB();
                status = (from  c in connectDB.Registers
                          where c.Volunteerid == volunteerid
                          select c
                          ).Count();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering request", ex);
            }
            return status;
        }
        public int Check(int id)
        {
            int status = 0;
            try
            {
                int check = listvolunteerid(id);
                if (check != 0)
                {
                    var connectDB = new ConnectDB();
                    var latestRegistrationDate = (from v in connectDB.Volunteers
                                                  join c in connectDB.Registers on v.Volunteerid equals c.Volunteerid
                                                  join r in connectDB.Requests on c.Requestid equals r.Requestid
                                                  where (r.RequestDate <= DateTime.Now && c.Quantity == 0)
                                                         && v.Volunteerid == id
                                                  orderby r.RequestDate descending
                                                  select r.RequestDate
                             ).Take(1)
                              .FirstOrDefault();
                    if (latestRegistrationDate == DateTime.MinValue)
                    {
                        var checks = (from v in connectDB.Volunteers
                                      join c in connectDB.Registers on v.Volunteerid equals c.Volunteerid
                                      join r in connectDB.Requests on c.Requestid equals r.Requestid
                                      where v.Volunteerid == id
                                      orderby r.RequestDate descending
                                      select r.RequestDate
                             ).Take(1)
                              .FirstOrDefault();
                        if (checks <= DateTime.Now.AddMonths(-3))
                        {
                            status = 1;
                        }
                        else
                        {
                            status = 0;
                        }
                    }
                    else
                    {
                        status = 1;
                    }

                }
                else
                {
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while registering request", ex);
            }
            return status;
        }

        public IEnumerable<Notification> GetNotifications(int id)
        {
            List <Notification> notifications = null;
            try
            {
                var connectDB = new ConnectDB();
                notifications = connectDB.Notification.Where(n => n.Userid == id).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while request", ex);
            }
            return notifications;
        }
        public void updatestatusnotification(Notification notification)
        {
            try
            {
                using (var connectDB = new ConnectDB())
                {
                    List<Notification> notifications = connectDB.Notification.Where(c => c.Userid == notification.Userid && c.status == 0).ToList();
                    if(notifications != null)
                    {
                        foreach(var item in notifications)
                        {
                            var existingNotification = connectDB.Notification.FirstOrDefault(n => n.NotificationId == item.NotificationId);
                            if (existingNotification != null)
                            {
                                existingNotification.status = notification.status;
                                connectDB.Entry(existingNotification).State = EntityState.Modified;
                                connectDB.SaveChanges();
                            }
                            else
                            {
                                throw new ArgumentException("Notification not found", nameof(notification.NotificationId));
                            }
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Notification not found", nameof(notification.NotificationId));
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while request", ex);
            }
        }
        public int countnotification(int id)
        {
            int total = 0;
            try
            {
                var connectDB = new ConnectDB();
                total = connectDB.Notification.Where(n => n.Userid == id && n.status == 0).Count();
            }catch(Exception ex)
            {
                throw new Exception("Error occurred while request", ex);
            }
            return total;
        }
        public void updateProfileVolunteer(Volunteers users)
        {
            try
            {
                var connectDB = new ConnectDB();
                var use = connectDB.Volunteers.FirstOrDefault(n => n.Volunteerid == users.Volunteerid);
                if (use != null)
                {
                    use.Birthdate= users.Birthdate;
                    use.Gender= users.Gender;
                    use.Fullname= users.Fullname;
                    use.CCCD= users.CCCD;
                    connectDB.Entry(use).State = EntityState.Modified;
                    connectDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int countRegister(int id)
        {
            int user = 0;
            try
            {
                var connectDB = new ConnectDB();
                user = (from u in connectDB.Registers
                        where u.Volunteerid == id && u.Quantity != 0
                        select u
                        ).Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
    }
}
