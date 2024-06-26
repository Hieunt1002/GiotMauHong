﻿using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class VolunteerRepository : IVolunteerRepository
    {

        public int Check(int id) => VolunteersDAO.Instance.Check(id);

        public int countnotification(int id) => VolunteersDAO.Instance.countnotification(id);

        public int countRegister(int id) => VolunteersDAO.Instance.countRegister(id);

        public IEnumerable<Notification> GetNotifications(int id) => VolunteersDAO.Instance.GetNotifications(id);

        public async Task regesterRequest(Registers registers) => await VolunteersDAO.Instance.regesterRequest(registers);

        public IEnumerable<ViewRequest> searchRequest(DateTime startdate, DateTime enddate, int volunteerid) => VolunteersDAO.Instance.searchRequest(startdate, enddate, volunteerid);

        public void updateProfileVolunteer(Volunteers users) => VolunteersDAO.Instance.updateProfileVolunteer(users);

        public void updatestatusnotification(Notification notification) => VolunteersDAO.Instance.updatestatusnotification(notification);
    }
}
