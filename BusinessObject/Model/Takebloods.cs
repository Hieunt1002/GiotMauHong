using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Takebloods
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Takebloodid { get; set; }
        [ForeignKey("Hospitals")]
        public int Hospitalid { get; set; }
        [ForeignKey("Bloodbank")]
        public int Bloodbankid { get; set; }
        public DateTime Datetake { get; set; }
        public int Status { get; set; }
        public virtual Hospitals Hospitals { get; set; }
        public virtual Bloodbank Bloodbank { get; set; }
        public virtual ICollection<QuantityTake> QuantityTake { get; set; } = new List<QuantityTake>();
    }
}
