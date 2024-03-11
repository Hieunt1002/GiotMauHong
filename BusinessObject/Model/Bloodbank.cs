using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Bloodbank
    {
        [Key, ForeignKey("Users")]
        public int Bloodbankid { get; set; }
        public string NameBloodbank { get; set;}
        public virtual Users Users { get; set; }
        public virtual ICollection<SendBlood> SendBloods { get; set; } = new List<SendBlood>();
        public virtual ICollection<Takebloods> Takebloods { get; set; } = new List<Takebloods>();
        public virtual ICollection<Hospitals> Hospitals { get; set; } = new List<Hospitals>();

    }
}
