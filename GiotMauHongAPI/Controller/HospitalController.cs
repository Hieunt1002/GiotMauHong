using Azure.Core;
using BusinessObject.Model;
using DataAccess.Model;
using DataAccess.Repository;
using GiotMauHongAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiotMauHongAPI.Controller
{
    [Route("api/Hopital")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private IHospitalRepository repository = new HospitalRepository();
        [HttpGet]
        [Route("listRequest")]
        public ActionResult<IEnumerable<Requests>> getlistrequest(int id)
        {
            try
            {
                var r = repository.GetRequestsByHospital(id);
                if(r == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(r);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("AddRequest")]
        public ActionResult AddRequest(ARequest aRequest)
        {
            try
            {
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
                    Address = aRequest.Address
                };
                repository.AddRequest(request);
                return Ok(request);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("updaterequest")]
        public ActionResult updaterequest(Urequest uRequest)
        {
            try
            {
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
                return Ok(request);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("deleterequest")]
        public ActionResult deleterequest(int id)
        {
            try
            {
                repository.DeleteRequest(id);
                return NoContent();
            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet]
        [Route("listvolunteerregister")] 
        public ActionResult<Requests> listvolunteerregister(int id)
        {
            try
            {
                return repository.listvolunteerregister(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
