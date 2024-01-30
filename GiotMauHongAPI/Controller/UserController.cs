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

namespace GiotMauHongAPI.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly AppSettings _appSettings;

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
                var hash = GetMD5(login.password);
                var admin = _repository.GetDefaultMember(login.email, hash);
                if (admin != null)
                {
                    var adminS = GenerateToken(admin);
                    return Ok(new ApiReponse
                    {
                        Success = true,
                        Message = "Authenticate success",
                        Data = adminS
                    });
                }
                else
                {
                    var user = _repository.Login(login.email, hash);
                    if (user == null)
                    {
                        return Ok(new ApiReponse
                        {
                            Success = false,
                            Message = "Invalid username/pass"
                        });
                    }
                    var token = GenerateToken(user);
                    return Ok(new ApiReponse
                    {
                        Success = true,
                        Message = "Authenticate success",
                        Data = token
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                if (user == null)
                    return NotFound();
                string hash = GetMD5(user.Password);
                var register = new Users
                {
                    Email = user.Email,
                    Password = hash,
                    Role = 1
                };

                _repository.AddUser(register);
                var users = _repository.getUserid(user.Email);
                var volun = new Volunteers
                {
                    Volunteerid = users.UserId
                };
                _repository.AddVolunteers(volun);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("ChangePass")]
        public ActionResult ChangePass(Changepass changepass)
        {
            try
            {
                if (changepass == null)
                    return NotFound();
                var check = _repository.checkpass(changepass.email, GetMD5(changepass.oldpassword));
                if (check == null)
                {
                    return Content("Old password is not correct");
                }
                else
                {
                    string hashedNewPassword = GetMD5(changepass.newpassword);
                    var change = new Users
                    {
                        UserId = check.UserId,
                        Email = changepass.email,
                        Password = hashedNewPassword,
                    };
                    _repository.ChangePass(change);

                    return Content("Password changed successfully");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
