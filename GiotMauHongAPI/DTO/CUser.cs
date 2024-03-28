using System.ComponentModel.DataAnnotations;

namespace GiotMauHongAPI.DTO
{
    public class CUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string CCCD { get; set; }
        public string Fullname { get; set; }
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
    }
    
}
