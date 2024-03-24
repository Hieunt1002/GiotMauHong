using BusinessObject.Model;
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
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminRepository repository = new AdminRepository();
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
        [Route("createbloodbank")]
        [Authorize]
        public ActionResult Register(ABloodBank user)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                string hash = GetMD5(user.Password);
                var register = new Users
                {
                    Img = user.Img,
                    Email = user.Email,
                    Password = hash,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    Ward = user.Ward,
                    District = user.District,
                    Address = user.Address,
                    Role = 3
                };

                repository.AddUser(register);
                var users = repository.GetUserByEmail(user.Email);
                var volun = new Bloodbank
                {
                    Bloodbankid = users.UserId,
                    NameBloodbank = user.NameBloodbank
                };
                repository.AddBloodBank(volun);
                return NoContent();
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
        [Route("listactive")]
        public ActionResult<IEnumerable<Activate>> listactive()
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                var a = repository.GetActivates();
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<IEnumerable<Activate>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = a
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
        [Route("lisbloodbank")]
        [Authorize]
        public ActionResult<IEnumerable<Users>> lisbloodbank()
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                var a = repository.GetListBloodbank();
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<IEnumerable<Users>>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = a
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
        [Route("getactiveid")]
        public ActionResult<Activate> getactiveid(int id)
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
                var a = repository.GetActivatesbyID(id);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<Activate>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = a
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
        [Route("addactive")]
        [Authorize]
        public ActionResult AddActive(AddActive nameactive)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                var a = new Activate
                {
                    NameActivate = nameactive.NameActivate,
                    Datepost = DateTime.Now,
                };
                repository.AddActive(a);
                int i = repository.GetActive();
                if (i != 0)
                {
                    foreach (var img in nameactive.aImgs)
                    {
                        var m = new Images
                        {
                            Activateid = i,
                            Img = img.Img,
                        };
                        repository.AddImgforActive(m);
                    }
                }
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = nameactive.NameActivate
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
        [Route("updateactive")]
        [Authorize]
        public ActionResult UpdateActive(UpdateActive nameactive)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (nameactive.ActiveId == 0)
                {
                    var error = errorResponse.CheckEmpty;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var a = new Activate
                {
                    Activateid = nameactive.ActiveId,
                    NameActivate = nameactive.NameActivate
                };
                repository.UpdateActive(a);
                foreach (var img in nameactive.uImgs)
                {
                    var m = new Images
                    {
                        ImgId = img.ImgId,
                        Img = img.Img,
                    };
                    repository.UpdateImgforActive(m);
                }
                var successResponse = config.SuccessMessages.RegistrationSuccess;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = nameactive.NameActivate
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
        [Route("deleteactive")]
        [Authorize]
        public ActionResult deleteactive(int id)
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
                repository.DeleteActive(id);
                var successResponse = config.SuccessMessages.RegistrationSuccess;
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
    }
}
