using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace GiotMauHongAPI.Controller
{
    [Route("api/Manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private IManagerRepository repository = new ManagerRepository();
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpPost]
        [Route("createhospital")]
        [Authorize]
        public ActionResult Register(AHospital user)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (user == null || user.Img == null || user.Email == null || user.Password == null || user.PhoneNumber == null || user.City == null || user.Ward == null || user.District == null || user.Address == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                string hash = GetMD5(user.Password);
                var register = new Users
                {
                    Img = user.Img,
                    Email = user.Email,
                    Password = hash,
                    PhoneNumber = user.PhoneNumber,
                    City= user.City,
                    Ward= user.Ward,
                    District= user.District,
                    Address= user.Address,
                    Role = 3
                };

                repository.AddUser(register);
                var users = repository.getUserid(user.Email);
                var volun = new Hospitals
                {
                    Hospitalid = users.UserId,
                    NameHospital = user.NameHospital,
                    Bloodbankid = user.bloodbankid
                };
                repository.AddHospital(volun);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user.Email
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
        [Route("listHospital")]
        [Authorize]
        public ActionResult<IEnumerable<Users>> GetHospital(int id) 
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
                var user = repository.GetListHospital(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Users>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user
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
        [Route("getInforHospitalsendbood")]
        [Authorize]
        public ActionResult<IEnumerable<InforHospitalDTO>> GetInforHospital(int id)
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
                var user = repository.GetInforHospitalDTOs(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<InforHospitalDTO>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user
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
        [Route("getblooddonationsession")]
        [Authorize]
        public ActionResult<IEnumerable<Requests>> getblooddonationsession(int id)
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
                var user = repository.GetRequestsByHospital(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Requests>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user
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
        [Route("getInforHospitaltakeblood")]
        [Authorize]
        public ActionResult<IEnumerable<InforHospitalDTO>> getInforHospitaltakeblood(int id)
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
                var user = repository.GetInforHospitalTakeBlood(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<InforHospitalDTO>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user
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
        [Route("updatestatussendblood")]
        [Authorize]
        public ActionResult updatestatussendblood(UpdateStatusDTO updateStatus)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateStatus.Id == 0 || updateStatus.Status == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var send = new SendBlood
                {
                    SendBloodid = updateStatus.Id,
                    Status = updateStatus.Status
                };
                repository.acceptSend(send);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = updateStatus.Id.ToString()
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
        [Route("updatestatustakeblood")]
        [Authorize]
        public ActionResult updatestatustakeblood(UpdateStatusDTO updateStatus)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateStatus.Id == 0 || updateStatus.Status == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var send = new Takebloods
                {
                    Takebloodid = updateStatus.Id,
                    Status = updateStatus.Status
                };
                repository.accepttake(send);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = updateStatus.Id.ToString()
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
        [Route("acceptRequest")]
        [Authorize]
        public ActionResult acceptRequest(UpdateStatusDTO updateStatus)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateStatus.Id == 0 || updateStatus.Status == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var send = new Requests
                {
                    Requestid = updateStatus.Id,
                    status = updateStatus.Status
                };
                repository.acceptRequest(send);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = updateStatus.Id.ToString()
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
        [Route("listRequestsByBloodbank")]
        [Authorize]
        public ActionResult<IEnumerable<Requests>> ListRequestsByBloodbank(int bloodbankid)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (bloodbankid == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var user = repository.ListRequestsByBloodbank(bloodbankid);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Requests>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = user
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
