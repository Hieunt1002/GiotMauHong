﻿using BusinessObject.Context;
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
        public IEnumerable<ViewRequest> searchRequest(DateTime startdate, DateTime enddate, string address)
        {
            List<ViewRequest> requests = null;
            try
            {
                var connectDB = new ConnectDB();
                requests = (from r in connectDB.Requests
                            join h in connectDB.Hospitals on r.Hospitalid equals h.Hospitalid
                            join d in connectDB.Registers on r.Requestid equals d.Requestid into registerGroup
                            from d in registerGroup.DefaultIfEmpty()
                            where r.RequestDate >= startdate && r.RequestDate <= enddate && r.City == address && r.status != 0
                            select new ViewRequest
                            {
                                Requestid = r.Requestid,
                                Hospitalid = r.Hospitalid,
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
                            }).ToList();
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
                    await MailUtils.SendMailGoogleSmtp("nguyenanh0978638@gmail.com",
                          u.Email,
                          "Đăng ký hiến máu",
                          "Chúc mừng bạn đã đăng ký thành công. Vui lòng đến đúng ngày và thời gian để tham gia hiến máu.",
                          "nguyenanh0978638@gmail.com", "kxnutqxwydifngae");
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
        public int Check(int id)
        {
            int status = 0;
            try
            {
                var connectDB = new ConnectDB();
                status = (from c in connectDB.Registers
                          join r in connectDB.Requests on c.Requestid equals r.Requestid
                          where ((r.RequestDate <= DateTime.Now && c.Quantity == 0) 
                          || r.RequestDate <= DateTime.Now.AddMonths(-3)) && c.Volunteerid == id
                          select c
                          ).Count();

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
            }catch(Exception ex)
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
                    var existingNotification = connectDB.Notification.FirstOrDefault(n => n.Userid == notification.Userid);
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
    }
}