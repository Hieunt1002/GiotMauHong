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
        [Route("searchrequest")]
        public ActionResult<IEnumerable<ViewRequest>> searchrequest(DateTime startdate, DateTime enddate, string address)
        {
            try
            {
                var s = repository.searchRequest(startdate, enddate, address);
                return Ok(s);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> registerRequest(registerRequests register)
        {
            if (register == null)
            {
                return BadRequest("Invalid data provided");
            }

            try
            {
                if (register.Volunteerid == 0 || register.Requestid == 0)
                {
                    return BadRequest("Volunteerid and Requestid must be provided");
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
                repository.addnotification(n);
                return Ok(r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
        [HttpGet]
        [Route("checkregister")]
        public ActionResult<int> check (int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid data provided");
                }
                var r = repository.Check(id);
                return Ok(r);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
        [HttpGet]
        [Route("notification")]
        public ActionResult<Notification> notification(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid data provided");
                }
                var r = repository.GetNotifications(id);
                return Ok(r);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
        [HttpPut]
        [Route("updatestatusnotification")]
        public ActionResult updatestatusnotification (int userid)
        {
            try
            {
                var n = new Notification
                {
                    Userid = userid,
                    status = 1
                };
                repository.updatestatusnotification(n);
                return Content("Successfully");
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
        [HttpGet]
        [Route("countnotification")]
        public ActionResult<int> countnotification (int id)
        {
            try
            {
                return repository.countnotification(id);
            }catch
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
    }
}
