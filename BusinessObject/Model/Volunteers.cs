using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Volunteers
    {
        [Key, ForeignKey("Users")]
        public int Volunteerid { get; set; }
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
        public string Fullname { get; set; }
        public string CCCD { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Registers> Registers { get; set; } = new List<Registers>();

    }
}
