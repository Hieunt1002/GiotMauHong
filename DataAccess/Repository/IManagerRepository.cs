using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IManagerRepository
    {
        void AddHospital(Hospitals hospitals);
        void AddUser(Users user);
        IEnumerable<Users> GetListHospital(int id);
        Users getUserid(string email);
        IEnumerable<InforHospitalDTO> GetInforHospitalDTOs(int id);
        IEnumerable<InforHospitalDTO> GetInforHospitalTakeBlood(int id);
        void acceptSend(SendBlood sendBlood);
        void accepttake(Takebloods takebloods);
        void acceptRequest(Requests requests);
        IEnumerable<Requests> GetRequestsByHospital(int id);
    }
}
