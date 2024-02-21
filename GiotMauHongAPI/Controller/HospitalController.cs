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
    }
}
