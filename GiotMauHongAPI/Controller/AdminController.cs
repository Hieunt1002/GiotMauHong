using BusinessObject.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Register(ABloodBank user)
        {
            try
            {
                if (user == null)
                    return NotFound();
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
                    Role = 2
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("listactive")]
        public ActionResult<IEnumerable<Activate>> listactive()
        {
            try
            {
                var a = repository.GetActivates();
                return Ok(a);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getactiveid")]
        public ActionResult<Activate> getactiveid(int id)
        {
            try
            {
                var a = repository.GetActivatesbyID(id);
                return Ok(a);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("addactive")]
        public ActionResult AddActive([FromBody] AddActive nameactive)
        {
            try
            {
                var a = new Activate
                {
                    NameActivate = nameactive.NameActivate,
                    Datepost = DateTime.Now,
                };
                repository.AddActive(a);
                int i = repository.GetActive();
                if(i != 0)
                {
                    foreach(var img in nameactive.aImgs){
                        var m = new Images
                        {
                            Activateid = i,
                            Img = img.Img,
                        };
                        repository.AddImgforActive(m);
                    }
                }
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
