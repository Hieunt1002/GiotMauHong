using BusinessObject.Model;
using DataAccess.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace GiotMauHongAPI.Controller
{
    [Route("api/volunteer")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        private IVolunteerRepository repository = new VolunteerRepository();
        private IUserRepository userRepository = new UserRepository();
        [HttpGet]
        [Route("searchrequest/{startdate}/{enddate}/{address}")]
        public ActionResult<IEnumerable<ViewRequest>> searchrequest(DateTime startdate, DateTime enddate, string address)
        {

            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (startdate == null || enddate == null)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                if (string.IsNullOrEmpty(address))
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var s = repository.searchRequest(startdate, enddate, address);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<ViewRequest>>
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
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> registerRequest(registerRequests register)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (register.Volunteerid == 0 || register.Requestid == 0)
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
                    Volunteerid = register.Volunteerid,
                    Requestid = register.Requestid,
                    Bloodtypeid = 1
                };
                await repository.regesterRequest(r);
                var n = new Notification
                {
                    Userid = r.Volunteerid,
                    Content = "Bạn đã đăng ký thành công xin vui lòng bạn đến đúng ngày để tham gia buổi hiến máu một cách trọn vẹn nhất",
                    Datepost = DateTime.Now.ToString(),
                    status = 0
                };
                userRepository.addnotification(n);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = r.Volunteerid
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
        [Route("checkregister")]
        public ActionResult<int> check (int id)
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
                else
                {
                    var r = repository.Check(id);
                    var successResponse = config.SuccessMessages.Successfully;
                    return Ok(new SuccessResponse<int>
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
        [HttpGet]
        [Route("notification")]
        public ActionResult<Notification> notification(int id)
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
                var r = repository.GetNotifications(id);
                var successResponse = config.SuccessMessages.Successfully;
                return Ok(new SuccessResponse<IEnumerable<Notification>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = r
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
        [Route("updatestatusnotification")]
        public ActionResult updatestatusnotification (int userid)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (userid == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var n = new Notification
                {
                    Userid = userid,
                    status = 1
                };
                repository.updatestatusnotification(n);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = userid.ToString()
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
        [Route("countnotification")]
        public ActionResult<int> countnotification (int id)
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
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = repository.countnotification(id)
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
