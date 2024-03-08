using BusinessObject.Model;
using DataAccess.DTO;
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
        void UpdateRequest(Requests request);
        void DeleteRequest(int id);
        Requests listvolunteerregister(int id);
        IEnumerable<Bloodtypes> GetBloodtypes();
        void UpdateRegister(Registers registers);
        Requests listhadvolunteerregister(int id);
        void AddSendBlood(SendBlood sendBlood);
        void AddTakeBlood(Takebloods takebloods);
        void QuantityTakeBlood(QuantityTake quantityTake);
        int takebookid(int hopitalid);
        int sendbookid(int hopitalid);
        IEnumerable<NumberBlood> listnumberBloods();
        void QuantitySendBlood(QuantitySend quantitySend);
        Requests GetRequestsByid(int id);
        void updatesendblood(QuantitySend sendBlood);
        SendBlood GetSendBloodbyid(int id);
        IEnumerable<SendBlood> GetSendBlood();
        void cancelsendblood(int id);
        void canceltakeblood(int id);
        IEnumerable<Takebloods> GetTakeBlood();
        IEnumerable<Takebloods> GetTakeBloodbyid(int id);
        void updatetakeblood(QuantityTake quantityTake);
        IEnumerable<NumberBloodDTO> listnumberblood();
        IEnumerable<SendBlood> GetSendBloodbyhospitalid(int id);
    }
}
