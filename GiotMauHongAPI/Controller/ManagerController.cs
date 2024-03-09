using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Register(AHospital user)
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
                    City= user.City,
                    Ward= user.Ward,
                    District= user.District,
                    Address= user.Address,
                    Role = 2
                };

                repository.AddUser(register);
                var users = repository.getUserid(user.Email);
                var volun = new Hospitals
                {
                    Hospitalid = users.UserId,
                    NameHospital = user.NameHospital
                };
                repository.AddHospital(volun);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("listHospital")]
        public ActionResult<IEnumerable<Users>> GetHospital() 
        {
            try
            {
                var user = repository.GetListHospital();
                if (user == null)
                    return Content("Not found");
                return Ok(user);
            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getInforHospitalsendbood")]
        public ActionResult<IEnumerable<InforHospitalDTO>> GetInforHospital(int id)
        {
            try
            {
                var user = repository.GetInforHospitalDTOs(id);
                if (user == null)
                    return Content("Not found");
                return Ok(user);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("getInforHospitaltakeblood")]
        public ActionResult<IEnumerable<InforHospitalDTO>> getInforHospitaltakeblood(int id)
        {
            try
            {
                var user = repository.GetInforHospitalTakeBlood(id);
                if (user == null)
                    return Content("Not found");
                return Ok(user);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updatestatussendblood")]
        public ActionResult updatestatussendblood(UpdateStatusDTO updateStatus)
        {
            try
            {
                var send = new SendBlood
                {
                    SendBloodid = updateStatus.Id,
                    Status = updateStatus.Status
                };
                repository.acceptSend(send);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updatestatustakeblood")]
        public ActionResult updatestatustakeblood(UpdateStatusDTO updateStatus)
        {
            try
            {
                var send = new Takebloods
                {
                    Takebloodid = updateStatus.Id,
                    Status = updateStatus.Status
                };
                repository.accepttake(send);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
