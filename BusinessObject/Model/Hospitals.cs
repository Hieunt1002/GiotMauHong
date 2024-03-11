using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Hospitals
    {
        [Key, ForeignKey("Users")]
        public int Hospitalid { get; set; }
        public string NameHospital { get; set; }
        [ForeignKey ("Bloodbank")]
        public int Bloodbankid { get; set; }
        public virtual Bloodbank Bloodbank { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Requests> Requests { get; set; } = new List<Requests>();
        public virtual ICollection<SendBlood> SendBloods { get; set; } = new List<SendBlood>();
        public virtual ICollection<Takebloods> Takebloods { get; set; } = new List<Takebloods>();

    }
}
