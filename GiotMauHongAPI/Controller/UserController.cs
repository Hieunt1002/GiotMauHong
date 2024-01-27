using BusinessObject.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace GiotMauHongAPI.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository repository = new UserRepository();
        private readonly AppSettings _appSettings;
        [HttpPost]
        [Route("login")]
        public ActionResult Login(Login login)
        {
            try
            {
                var user = repository.Login(login.email, login.password);
                if (user == null)
                {
                    return Ok(new ApiReponse
                    {
                        Success = false,
                        Message = "Invalid username/pass"
                    });
                }
                var token = GenerateToken(user, user.Role);
                return Ok(new ApiReponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = token
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        private string GenerateToken(Users user, int isAdmin)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.ScretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Role", isAdmin.ToString()),
                    new Claim("Id", user.UserId.ToString()),

                    //roles

                    new Claim("TokenId", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, isAdmin == 1 ? "Volunteers" : isAdmin == 2 ? "Hospitals" : "Bloodbank")
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
        [HttpPost]
        public ActionResult Rigister(CUser user)
        {
            try
            {
                //if (user == null) return NotFound();
                //var register = new User
                //{
                //    uImg = user.uImg,
                //    email = user.email,
                //    password = user.password,
                //    firstName = user.firstName,
                //    lastName = user.lastName,
                //    phoneNumber = user.phoneNumber,
                //    roleId = user.roleId,
                //    address = user.address
                //};
                //repository.AddUser(register);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }

}
