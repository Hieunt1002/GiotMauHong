using BusinessObject.Model;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        public void AddHospital(Hospitals hospitals) => ManagerDAO.Instance.AddHospital(hospitals);

        public void AddUser(Users user) => ManagerDAO.Instance.AddUser(user);

        public IEnumerable<Users> GetListHospital() => ManagerDAO.Instance.GetListHospital();

        public Users getUserid(string email) => ManagerDAO.Instance.getUserid(email);
    }
}
