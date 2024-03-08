using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class InforHospitalDTO
    {
        public int UserId { get; set; }
        public string Img { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string NameHospital { get; set; }
        public List<NumberBloodDTO> NumberBlood { get; set; } = new List<NumberBloodDTO>();
        public int status { get; set; }
    }
}
