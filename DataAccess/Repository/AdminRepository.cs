using BusinessObject.Model;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public void AddActive(Activate activate) => AdminDAO.Instance.AddActive(activate);

        public void AddBloodBank(Bloodbank bloodbank) => AdminDAO.Instance.AddBloodBank(bloodbank);

        public void AddImgforActive(Images images) => AdminDAO.Instance.AddImgforActive(images);

        public void AddUser(Users user) => AdminDAO.Instance.AddUser(user);

        public void DeleteActive(int id) => AdminDAO.Instance.DeleteActive(id);

        public IEnumerable<Activate> GetActivates() => AdminDAO.Instance.GetActivates();

        public Activate GetActivatesbyID(int id) => AdminDAO.Instance.GetActivatesbyID(id);

        public int GetActive() => AdminDAO.Instance.GetActive();

        public Users GetUserByEmail(string email) => AdminDAO.Instance.GetUserByEmail(email);

        public void UpdateActive(Activate activate) => AdminDAO.Instance.UpdateActive(activate);

        public void UpdateImgforActive(Images images) => AdminDAO.Instance.UpdateImgforActive(images);
    }
}
