using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Img { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set;}
        public string Ward { get; set;}
        public string District { get; set;}
        public string Address { get; set;}
        [Required]
        public int Role { get; set;}
        public virtual Volunteers Volunteers { get; set; }
        public virtual Hospitals Hospitals { get; set; }
        public virtual Bloodbank Bloodbank { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
