using BusinessObject.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using DataAccess.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace GiotMauHongAPI.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;
        private IHospitalRepository repositoryHospital = new HospitalRepository();
        private IVolunteerRepository repositoryVolunteer = new VolunteerRepository();
        private IManagerRepository repositorybloodbank = new ManagerRepository();

        public UserController(IUserRepository repository, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _repository = repository;
            _appSettings = optionsMonitor.CurrentValue;
        }
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
        [Route("login")]
        public ActionResult Login(Login login)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (login == null || string.IsNullOrEmpty(login.email) || string.IsNullOrEmpty(login.password))
                {
                    var error = login == null ? errorResponse.BadRequest : errorResponse.EmailPassword;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                else if (!IsValidEmail(login.email))
                {
                    var error = errorResponse.InvalidEmailFormat;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }

                else if (login.password.Length < 6)
                {
                    var error = errorResponse.PasswordLength;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                else
                {
                    var hash = GetMD5(login.password);
                    var admin = _repository.GetDefaultMember(login.email, hash);
                    if (admin != null)
                    {
                        var adminS = GenerateToken(admin);
                        return Ok(new ApiReponse<Users>
                        {
                            Success = true,
                            Message = "Authenticate success",
                            Token = adminS,
                            Data = new Users
                            {
                                Email = "admin@gmail.com",
                                Role = 0
                            }
                        });
                    }
                    else
                    {
                        var user = _repository.Login(login.email, hash);
                        if (user == null)
                        {
                            return BadRequest(new ApiReponse<string>
                            {
                                Success = false,
                                Message = "Invalid username/pass"
                            });
                        }
                        var token = GenerateToken(user);
                        return Ok(new ApiReponse<Users>
                        {
                            Success = true,
                            Message = "Authenticate success",
                            Token = token,
                            Data = new Users
                            {
                                UserId = user.UserId,
                                Email = user.Email,
                                Img = user.Img,
                                Role = user.Role
                            }
                        });
                    }
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
        private string GenerateToken(Users user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.ScretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Role", user.Role.ToString()),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role == 1 ? "Volunteers" : user.Role == 2 ? "Hospitals" : user.Role == 3 ? "Bloodbank" : "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

        [HttpPost]
        [Route("register")]
        public ActionResult Register(CUser user)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                string hash = GetMD5(user.Password);
                var register = new Users
                {
                    Email = user.Email,
                    Password = hash,
                    Img = "",
                    PhoneNumber = "",
                    City = "",
                    Ward= "",
                    District= "",
                    Address= "",
                    Role = 1,
                    deactive = 1
                };

                _repository.AddUser(register);
                var users = _repository.getUserid(user.Email);
                var volun = new Volunteers
                {
                    Volunteerid = users.UserId,
                    CCCD = "000000000001",
                    Fullname = "null",
                    Gender = 0
                };
                _repository.AddVolunteers(volun);

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

        [HttpPut]
        [Route("ChangePass")]
        [Authorize]
        public ActionResult ChangePass(Changepass changepass)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var check = _repository.checkpass(changepass.email, GetMD5(changepass.oldpassword));
                string hashedNewPassword = GetMD5(changepass.newpassword);
                var change = new Users
                {
                    UserId = check.UserId,
                    Email = changepass.email,
                    Password = hashedNewPassword,
                };
                _repository.ChangePass(change);
                var successResponse = config.SuccessMessages.ChangePassword;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = change.Email
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
        [Route("historyvolunteers")]
        [Authorize]
        public ActionResult<ViewHistory> Historyvolunteers(int id)
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
                return Ok(new SuccessResponse<ViewHistory>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = _repository.gethistory(id)
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
        [Route("profile")]
        [Authorize]
        public ActionResult<Users> profile(int id)
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
                return Ok(new SuccessResponse<Users>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = _repository.getProfile(id)
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
        [Route("forgotpass/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (email == null)
                {
                    var error = errorResponse.EmailRequired;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                else if (!IsValidEmail(email))
                {
                    var errorResponses = config.ErrorMessages.BadRequest;
                    return BadRequest(new ErrorMessage
                    {
                        StatusCode = errorResponses.StatusCode,
                        Message = errorResponses.Message,
                        ErrorDetails = errorResponses.ErrorDetails
                    });
                }

                var token = GenerateTokenForgot(email);
                var resetLink = "https://localhost:3000/resetpassword/" + token;
                var emailContent = GetResetPasswordEmailContent(resetLink);
                var forgot = await _repository.forgotpass(email, emailContent);

                var successResponse = config.SuccessMessages.ResetPasswordEmailSent;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = token
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


        private string GetResetPasswordEmailContent(string resetLink)
        {
            string emailContent = @"<!DOCTYPE html>
                            <html lang='en'>
                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                <title>Reset Password</title>
                            </head>
                            <body>
                                <p>Dear User,</p>
                                <p>You recently requested to reset your password. Please click the link below to reset your password:</p>
                                <p>Enter the link to change your password <a href='" + resetLink + @"'>Link</a></p>
                                <p>If you did not request a password reset, please ignore this email. Your password will remain unchanged.</p>
                                <p>Best regards,</p>
                                <p>YourApp Team</p>
                            </body>
                            </html>";
            return emailContent;
        }
        private string GenerateTokenForgot(string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.ScretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
        [HttpPut]
        [Route("resetPass")]
        [Authorize]
        public ActionResult resetPass(resetpassword resetpassword)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");
                string hashedNewPassword = GetMD5(resetpassword.password);
                var change = new Users
                {
                    Email = resetpassword.email,
                    Password = hashedNewPassword,
                };
                _repository.ChangePass(change);
                var successResponse = config.SuccessMessages.ChangePassword;
                return Ok(new SuccessResponse<string>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = change.Email
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
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Kiểm tra định dạng của email sử dụng regex
                string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                return Regex.IsMatch(email, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        
        [HttpPut]
        [Route("updateProfileVolunteer")]
        [Authorize]
        public ActionResult UpdateProfile(UpdateProfileDTO updateProfile)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateProfile == null)
                {
                    var error = updateProfile == null ? errorResponse.BadRequest : errorResponse.EmailPassword;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var user = new Users
                {
                    UserId= updateProfile.UserId,
                    Img = updateProfile.Img,
                    PhoneNumber= updateProfile.PhoneNumber,
                    City= updateProfile.City,
                    Ward= updateProfile.Ward,
                    District= updateProfile.District,
                    Address= updateProfile.Address
                };
                var volunteer = new Volunteers
                {
                    Volunteerid = updateProfile.UserId,
                    Birthdate = updateProfile.Birthdate,
                    Gender = updateProfile.Gender,
                    Fullname = updateProfile.Fullname,
                    CCCD = updateProfile.CCCD
                };
                _repository.updateProfile(user);
                repositoryVolunteer.updateProfileVolunteer(volunteer);
                var successResponse = config.SuccessMessages.ChangePassword;
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
        [HttpPut]
        [Route("updateProfileHospital")]
        [Authorize]
        public ActionResult updateProfileHospital(UpdateHospitalDTO updateProfile)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateProfile == null)
                {
                    var error = updateProfile == null ? errorResponse.BadRequest : errorResponse.EmailPassword;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var user = new Users
                {
                    UserId = updateProfile.UserId,
                    Img = updateProfile.Img,
                    PhoneNumber = updateProfile.PhoneNumber,
                    City = updateProfile.City,
                    Ward = updateProfile.Ward,
                    District = updateProfile.District,
                    Address = updateProfile.Address
                };
                var volunteer = new Hospitals
                {
                    Hospitalid = updateProfile.UserId,
                    NameHospital = updateProfile.NameHospital,
                };
                _repository.updateProfile(user);
                repositoryHospital.updateProfileHospotal(volunteer);
                var successResponse = config.SuccessMessages.ChangePassword;
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
        [HttpPut]
        [Route("updateProfilebloodbank")]
        [Authorize]
        public ActionResult updateProfilebloodbank(updatebloodbank updateProfile)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");

                var errorResponse = config.ErrorMessages;
                if (updateProfile == null)
                {
                    var error = updateProfile == null ? errorResponse.BadRequest : errorResponse.EmailPassword;
                    return StatusCode(error.StatusCode, new ErrorMessage
                    {
                        StatusCode = error.StatusCode,
                        Message = error.Message,
                        ErrorDetails = error.ErrorDetails
                    });
                }
                var user = new Users
                {
                    UserId = updateProfile.UserId,
                    Img = updateProfile.Img,
                    PhoneNumber = updateProfile.PhoneNumber,
                    City = updateProfile.City,
                    Ward = updateProfile.Ward,
                    District = updateProfile.District,
                    Address = updateProfile.Address
                };
                var volunteer = new Bloodbank
                {
                    Bloodbankid = updateProfile.UserId,
                    NameBloodbank = updateProfile.NameBloodbank,
                };
                _repository.updateProfile(user);
                repositorybloodbank.updateProfileBloodbank(volunteer);
                var successResponse = config.SuccessMessages.ChangePassword;
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
        [HttpPut]
        [Route("updatedeactive")]
        public ActionResult updateDeactive(int userid)
        {
            try
            {
                var config = Config.LoadFromFile("appsettings.json");
                var successResponse = config.SuccessMessages.Successfully;
                var user = new Users
                {
                    UserId = userid,
                    deactive = 0
                };
                _repository.updateDeactive(user);
                return Ok(new SuccessResponse<int>
                {
                    StatusCode = successResponse.StatusCode,
                    Message = successResponse.Message,
                    Data = userid
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
