using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IAdminRepository
    {
        IEnumerable<Activate> GetActivates();
        void AddActive(Activate activate);
        void AddImgforActive(Images images);
        void UpdateImgforActive(Images images);
        void UpdateActive(Activate activate);
        void DeleteActive(int id);
        Activate GetActivatesbyID(int id);
        void AddBloodBank(Bloodbank bloodbank);
        void AddUser(Users user);
        Users GetUserByEmail(string email);
        int GetActive();
        IEnumerable<Users> GetListBloodbank();
    }
}
