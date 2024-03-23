using BusinessObject.Model;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IVolunteerRepository
    {
        IEnumerable<ViewRequest> searchRequest(DateTime startdate, DateTime enddate, int volunteerid);
        Task regesterRequest(Registers registers);
        int Check(int id);
        IEnumerable<Notification> GetNotifications(int id);
        void updatestatusnotification(Notification notification);
        int countnotification(int id);
        void updateProfileVolunteer(Volunteers users);
        int countRegister(int id);
    }
}
