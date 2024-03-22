using System.ComponentModel.DataAnnotations;

namespace GiotMauHongAPI.DTO
{
    public class UpdateProfileDTO
    {
        public int UserId { get; set; }
        public string Img { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
        public string Fullname { get; set; }
        public string CCCD { get; set; }

    }
}
