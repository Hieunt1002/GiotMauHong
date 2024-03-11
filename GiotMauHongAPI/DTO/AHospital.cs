using System.ComponentModel.DataAnnotations;

namespace GiotMauHongAPI.DTO
{
    public class AHospital
    {
        public int bloodbankid { get; set; }
        public string Img { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string NameHospital { get; set; }
    }
}
