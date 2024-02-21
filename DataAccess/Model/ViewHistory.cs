using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ViewHistory
    {
        public int UserId { get; set; }
        public string Img { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public Volunteers Volunteers { get; set; }
    }
}
