using BusinessObject.Model;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IHospitalRepository
    {
        IEnumerable<Requests> GetRequestsByHospital(int id);
        void AddRequest(Requests request);
    }
}
