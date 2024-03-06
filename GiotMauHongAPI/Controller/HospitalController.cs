using Azure.Core;
using BusinessObject.Model;
using DataAccess.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiotMauHongAPI.Controller
{
    [Route("api/Hopital")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private IUserRepository userRepository = new UserRepository();
        private IHospitalRepository repository = new HospitalRepository();
        [HttpGet]
        [Route("listRequest")]
        public ActionResult<IEnumerable<Requests>> getlistrequest(int id)
        {
            try
            {
                var r = repository.GetRequestsByHospital(id);
                if (r == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(r);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("AddRequest")]
        public ActionResult AddRequest(ARequest aRequest)
        {
            try
            {
                var request = new Requests
                {
                    Hospitalid = aRequest.Hospitalid,
                    RequestDate = aRequest.RequestDate,
                    quantity = aRequest.quantity,
                    Contact = aRequest.Contact,
                    Starttime = aRequest.Starttime,
                    Endtime = aRequest.Endtime,
                    City = aRequest.City,
                    Ward = aRequest.Ward,
                    District = aRequest.District,
                    Address = aRequest.Address
                };
                repository.AddRequest(request);
                var n = new Notification
                {
                    Userid = aRequest.Hospitalid,
                    Content = "Bạn đã đăng ký lịch hiển máu thành công",
                    Datepost = DateTime.Now.ToString(),
                    status = 0
                };
                userRepository.addnotification(n);
                return Ok(request);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updaterequest")]
        public ActionResult updaterequest(Urequest uRequest)
        {
            try
            {
                var request = new Requests
                {
                    Requestid = uRequest.Requestid,
                    RequestDate = uRequest.RequestDate,
                    quantity = uRequest.quantity,
                    Contact = uRequest.Contact,
                    Starttime = uRequest.Starttime,
                    Endtime = uRequest.Endtime,
                    City = uRequest.City,
                    Ward = uRequest.Ward,
                    District = uRequest.District,
                    Address = uRequest.Address
                };
                repository.UpdateRequest(request);
                return Ok(request);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("deleterequest")]
        public ActionResult deleterequest(int id)
        {
            try
            {
                repository.DeleteRequest(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("listvolunteerregister")]
        public ActionResult<Requests> listvolunteerregister(int id)
        {
            try
            {
                return repository.listvolunteerregister(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("bloodtype")]
        public ActionResult<IEnumerable<Bloodbank>> getBloodtype()
        {
            try
            {
                return Ok(repository.GetBloodtypes());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updateregister")]
        public ActionResult updateregister(URegister uRegister)
        {
            try
            {
                var r = new Registers
                {
                    RegisterId = uRegister.RegisterId,
                    Quantity = uRegister.Quantity,
                    Bloodtypeid = uRegister.Bloodtypeid,
                };
                repository.UpdateRegister(r);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("addsendblood")]
        public ActionResult addsendblood(AddSendBloodDTO addSendBloodDTO)
        {
            try
            {
                var send = new SendBlood
                {
                    Hospitalid = addSendBloodDTO.Hospitalid,
                    Bloodbankid = addSendBloodDTO.Bloodbankid,
                    Datesend = addSendBloodDTO.Datesend,
                    Status = 0
                };
                repository.AddSendBlood(send);
                int id = repository.sendbookid(addSendBloodDTO.Hospitalid);
                if (id == 0)
                {
                    return Content("Not found");
                }
                else
                {
                    foreach (var s in addSendBloodDTO.QuantitySend)
                    {
                        var q = new QuantitySend
                        {
                            numberbloodid = s.numberbloodid,
                            SendBloodid = id,
                            Bloodtypeid = s.Bloodtypeid,
                            quantity = s.quantity
                        };
                        repository.QuantitySendBlood(q);
                    }
                }
                var n = new Notification
                {
                    Userid = addSendBloodDTO.Hospitalid,
                    Content = "Bạn đã đăng ký gửi máu thành công",
                    Datepost = DateTime.Now.ToString(),
                    status = 0
                };
                userRepository.addnotification(n);
                return Content("Successfully");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("addtakeblood")]
        public ActionResult addtakeblood(AddTakeBloodDTO addTakeBloodDTO)
        {
            try
            {
                var send = new Takebloods
                {
                    Hospitalid = addTakeBloodDTO.Hospitalid,
                    Bloodbankid = addTakeBloodDTO.Bloodbankid,
                    Datetake = addTakeBloodDTO.Datetake,
                    Status = 0
                };
                repository.AddTakeBlood(send);
                int id = repository.takebookid(addTakeBloodDTO.Hospitalid);
                if (id == 0)
                {
                    return Content("Not found");
                }
                else
                {
                    foreach (var s in addTakeBloodDTO.QuantityTake)
                    {
                        var q = new QuantityTake
                        {
                            numberbloodid = s.numberbloodid,
                            Takebloodid = id,
                            Bloodtypeid = s.Bloodtypeid,
                            quantity = s.quantity
                        };
                        repository.QuantityTakeBlood(q);
                    }
                }
                var n = new Notification
                {
                    Userid = addTakeBloodDTO.Hospitalid,
                    Content = "Bạn đã đăng ký lấy máu thành công",
                    Datepost = DateTime.Now.ToString(),
                    status = 0
                };
                userRepository.addnotification(n);
                return Content("Successfully");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updatesendblood")]
        public ActionResult updatesendblood (List<UpdateQuantitySendDTO> updateQuantitySendDTOs)
        {
            try
            {
                foreach (var s in updateQuantitySendDTOs.ToList())
                {
                    var q = new QuantitySend
                    {
                        quantitysendid = s.quantitysendid,
                        numberbloodid = s.numberbloodid,
                        Bloodtypeid = s.Bloodtypeid,
                        quantity = s.quantity
                    };
                    repository.updatesendblood(q);
                }
                return Content("Successfully");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updatetakeblood")]
        public ActionResult updatetakeblood(List<UpdateQuantityTakeDTO> updateQuantityTakeDTOs)
        {
            try
            {
                foreach (var s in updateQuantityTakeDTOs.ToList())
                {
                    var q = new QuantityTake
                    {
                        quantitytakeid = s.quantitytakeid,
                        numberbloodid = s.numberbloodid,
                        Bloodtypeid = s.Bloodtypeid,
                        quantity = s.quantity
                    };
                    repository.updatetakeblood(q);
                }
                return Content("Successfully");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("cancelsend")]
        public ActionResult cancelsend(int id)
        {
            try
            {
                repository.cancelsendblood(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("canceltake")]
        public ActionResult canceltake(int id)
        {
            try
            {
                repository.canceltakeblood(id);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getsendblood")]
        public ActionResult<IEnumerable<SendBlood>> getlistsendblood()
        {
            try
            {
                var s = repository.GetSendBlood();
                return Ok(s);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getlistsendbloodbyid")]
        public ActionResult<SendBlood> getlistsendbloodbyid(int id)
        {
            try
            {
                var s = repository.GetSendBloodbyid(id);
                return Ok(s);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("gettakeblood")]
        public ActionResult<IEnumerable<SendBlood>> gettakeblood()
        {
            try
            {
                var s = repository.GetTakeBlood();
                return Ok(s);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getlisttakebloodbyid")]
        public ActionResult<SendBlood> getlisttakebloodbyid(int id)
        {
            try
            {
                var s = repository.GetTakeBloodbyid(id);
                return Ok(s);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("listnumberblood")]
        public ActionResult<IEnumerable<NumberBlood>> listnumberblood()
        {
            try
            {
                return Ok(repository.listnumberBloods());  
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
