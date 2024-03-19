using Azure.Core;
using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult<IEnumerable<Requests>> getlistrequest(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var r = repository.GetRequestsByHospital(id);
                if (r == null)
                {
                    return NotFound();
                }
                else
                {
                    var successResponse = config.SuccessMessages.Successfully;
                    return Ok(new SuccessResponse<IEnumerable<Requests>>
                    {
                        StatusCode = successResponse.StatusCode,
                        Message = successResponse.Message,
                        Data = r
                    });
                }
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPost]
        [Route("AddRequest")]
        [Authorize]
        public ActionResult AddRequest(ARequest aRequest)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (aRequest == null || aRequest.Hospitalid == 0 || aRequest.RequestDate == null ||
                    aRequest.quantity == 0 || aRequest.Contact == null || aRequest.Starttime == null ||
                    aRequest.Endtime == null || aRequest.City == null || aRequest.Ward == null || aRequest.District == null || aRequest.Address == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
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
                    Address = aRequest.Address,
                    status = 0
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = aRequest.Hospitalid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPut]
        [Route("updaterequest")]
        [Authorize]
        public ActionResult updaterequest(Urequest uRequest)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (uRequest == null || uRequest.Requestid == 0 || uRequest.RequestDate == null ||
                    uRequest.quantity == 0 || uRequest.Contact == null || uRequest.Starttime == null ||
                    uRequest.Endtime == null || uRequest.City == null || uRequest.Ward == null || uRequest.District == null || uRequest.Address == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = uRequest.Requestid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpDelete]
        [Route("deleterequest")]
        [Authorize]
        public ActionResult deleterequest(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                repository.DeleteRequest(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = id
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("listvolunteerregister")]
        [Authorize]
        public ActionResult<Requests> listvolunteerregister(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<Requests>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.listvolunteerregister(id)
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("bloodtype")]
        [Authorize]
        public ActionResult<IEnumerable<Bloodtypes>> getBloodtype()
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Bloodtypes>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.GetBloodtypes()
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPut]
        [Route("updateregister")]
        [Authorize]
        public ActionResult updateregister(URegister uRegister)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (uRegister == null || uRegister.RegisterId == 0 || uRegister.Quantity == 0 || uRegister.Bloodtypeid == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var r = new Registers
                {
                    RegisterId = uRegister.RegisterId,
                    Quantity = uRegister.Quantity,
                    Bloodtypeid = uRegister.Bloodtypeid,
                };
                repository.UpdateRegister(r);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = uRegister.RegisterId
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPost]
        [Route("addsendblood")]
        [Authorize]
        public ActionResult addsendblood(AddSendBloodDTO addSendBloodDTO)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (addSendBloodDTO == null || addSendBloodDTO.Hospitalid == 0  || 
                    addSendBloodDTO.Datesend == null || addSendBloodDTO.QuantitySend == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var bloodbank = userRepository.getProfile(addSendBloodDTO.Hospitalid);
                var send = new SendBlood
                {
                    Hospitalid = addSendBloodDTO.Hospitalid,
                    Bloodbankid = bloodbank.Hospitals.Bloodbankid,
                    Datesend = addSendBloodDTO.Datesend,
                    Status = 0
                };
                repository.AddSendBlood(send);
                int id = repository.sendbookid(addSendBloodDTO.Hospitalid);
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = addSendBloodDTO.Hospitalid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPost]
        [Route("addtakeblood")]
        [Authorize]
        public ActionResult addtakeblood(AddTakeBloodDTO addTakeBloodDTO)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (addTakeBloodDTO == null || addTakeBloodDTO.Hospitalid == 0 || 
                    addTakeBloodDTO.Datetake == null || addTakeBloodDTO.QuantityTake == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var bloodbank = userRepository.getProfile(addTakeBloodDTO.Hospitalid);
                var send = new Takebloods
                {
                    Hospitalid = addTakeBloodDTO.Hospitalid,
                    Bloodbankid = bloodbank.Hospitals.Bloodbankid,
                    Datetake = addTakeBloodDTO.Datetake,
                    Status = 0
                };
                repository.AddTakeBlood(send);
                int id = repository.takebookid(addTakeBloodDTO.Hospitalid);
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = addTakeBloodDTO.Hospitalid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPut]
        [Route("updatesendblood")]
        [Authorize]
        public ActionResult updatesendblood (List<UpdateQuantitySendDTO> updateQuantitySendDTOs)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateQuantitySendDTOs == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = updateQuantitySendDTOs[0].quantitysendid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpPut]
        [Route("updatetakeblood")]
        [Authorize]
        public ActionResult updatetakeblood(List<UpdateQuantityTakeDTO> updateQuantityTakeDTOs)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateQuantityTakeDTOs == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
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
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = updateQuantityTakeDTOs[0].quantitytakeid
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpDelete]
        [Route("cancelsend")]
        [Authorize]
        public ActionResult cancelsend(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                repository.cancelsendblood(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = id
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpDelete]
        [Route("canceltake")]
        [Authorize]
        public ActionResult canceltake(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                repository.canceltakeblood(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = id
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("getsendblood")]
        [Authorize]
        public ActionResult<IEnumerable<SendBlood>> getlistsendblood(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var s = repository.GetSendBlood(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<SendBlood>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = s
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("getlistsendbloodbyid")]
        [Authorize]
        public ActionResult<SendBlood> getlistsendbloodbyid(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var s = repository.GetSendBloodbyid(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<SendBlood>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = s
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("gettakeblood")]
        [Authorize]
        public ActionResult<IEnumerable<Takebloods>> gettakeblood(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var s = repository.GetTakeBlood(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Takebloods>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = s
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("getlisttakebloodbyid")]
        [Authorize]
        public ActionResult<IEnumerable<Takebloods>> getlisttakebloodbyid(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var s = repository.GetTakeBloodbyid(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Takebloods>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = s
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("listnumberblood")]
        public ActionResult<IEnumerable<NumberBlood>> listnumberblood()
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<NumberBlood>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.listnumberBloods()
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("displaysremainingblood")]
        [Authorize]
        public ActionResult<IEnumerable<NumberBloodDTO>> displaysremainingblood(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<NumberBloodDTO>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.listnumberblood(id)
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
        [HttpGet]
        [Route("getSendBloodbyhospitalid")]
        [Authorize]
        public ActionResult<IEnumerable<SendBlood>> GetSendBloodbyhospitalid(int id)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (id == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<SendBlood>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.GetSendBloodbyhospitalid(id)
                });
            }
            catch
            {
                var errorResponse = Config.LoadFromFile("appsettings.json").ErrorMessages.InternalServerError;
                return StatusCode(errorResponse.StatusCode, new ErrorMessage
                {
                    StatusCode = errorResponse.StatusCode,
                    Message = errorResponse.Message,
                    ErrorDetails = errorResponse.ErrorDetails
                });
            }
        }
    }
}
