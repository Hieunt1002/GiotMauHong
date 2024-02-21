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

        public IEnumerable<Requests> GetRequestsByHospital(int id) => HospitalDAO.Instance.GetRequestsByHospital(id);
    }
}
