using System.ComponentModel.DataAnnotations;

namespace GiotMauHongAPI.DTO
{
    public class Changepass
    {
        public string email { get; set; }

        public string oldpassword { get; set; }

        public string newpassword { get; set; }
    }
}
