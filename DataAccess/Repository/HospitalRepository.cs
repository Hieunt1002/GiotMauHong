using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HospitalRepository : IHospitalRepository
    {
        public void AddRequest(Requests request) => HospitalDAO.Instance.AddRequest(request);

        public void DeleteRequest(int id) => HospitalDAO.Instance.DeleteRequest(id);

        public IEnumerable<Bloodtypes> GetBloodtypes() => HospitalDAO.Instance.GetBloodtypes();
        public IEnumerable<Requests> GetRequestsByHospital(int id) => HospitalDAO.Instance.GetRequestsByHospital(id);

        public Requests listvolunteerregister(int id) => HospitalDAO.Instance.listvolunteerregister(id);

        public void UpdateRegister(Registers registers) => HospitalDAO.Instance.UpdateRegister(registers);

        public void UpdateRequest(Requests request) => HospitalDAO.Instance.UpdateRequest(request);

    }   
}
