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

        public void AddSendBlood(SendBlood sendBlood) => HospitalDAO.Instance.AddSendBlood(sendBlood);

        public void AddTakeBlood(Takebloods takebloods) => HospitalDAO.Instance.AddTakeBlood(takebloods);

        public void cancelsendblood(int id) => HospitalDAO.Instance.cancelsendblood(id);

        public void canceltakeblood(int id) => HospitalDAO.Instance.canceltakeblood(id);

        public void DeleteRequest(int id) => HospitalDAO.Instance.DeleteRequest(id);

        public IEnumerable<Bloodtypes> GetBloodtypes() => HospitalDAO.Instance.GetBloodtypes();

        public IEnumerable<Requests> GetRequestsByHospital(int id) => HospitalDAO.Instance.GetRequestsByHospital(id);

        public Requests GetRequestsByid(int id) => HospitalDAO.Instance.GetRequestsByid(id);

        public IEnumerable<SendBlood> GetSendBlood() => HospitalDAO.Instance.GetSendBlood();

        public SendBlood GetSendBloodbyid(int id) => HospitalDAO.Instance.GetSendBloodbyid(id);

        public IEnumerable<Takebloods> GetTakeBlood() => HospitalDAO.Instance.GetTakeBlood();

        public IEnumerable<Takebloods> GetTakeBloodbyid(int id) => HospitalDAO.Instance.GetTakeBloodbyid(id);

        public Requests listhadvolunteerregister(int id) => HospitalDAO.Instance.listvolunteerregister(id);

        public IEnumerable<NumberBlood> listnumberBloods() => HospitalDAO.Instance.listnumberBloods();

        public Requests listvolunteerregister(int id) => HospitalDAO.Instance.listvolunteerregister(id);

        public void QuantitySendBlood(QuantitySend quantitySend) => HospitalDAO.Instance.QuantitySendBlood(quantitySend);

        public void QuantityTakeBlood(QuantityTake quantityTake) => HospitalDAO.Instance.QuantityTakeBlood(quantityTake);

        public int sendbookid(int hopitalid) => HospitalDAO.Instance.sendbookid(hopitalid);

        public int takebookid(int hopitalid) => HospitalDAO.Instance.takebookid(hopitalid);

        public void UpdateRegister(Registers registers) => HospitalDAO.Instance.UpdateRegister(registers);

        public void UpdateRequest(Requests request) => HospitalDAO.Instance.UpdateRequest(request);

        public void updatesendblood(QuantitySend sendBlood) => HospitalDAO.Instance.updatesendblood(sendBlood);

        public void updatetakeblood(QuantityTake quantityTake) => HospitalDAO.Instance.updatetakeblood(quantityTake);
    }   
}
