using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class SendBlood
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SendBloodid { get; set; }

        [ForeignKey("Hospitals")]
        public int Hospitalid { get; set; }

        [ForeignKey("Bloodbank")]
        public int Bloodbankid { get; set; }

        [ForeignKey("Bloodtypes")]
        public int Bloodtypeid { get; set; }
        public int Quantity { get; set; }
        public DateTime Datesend { get; set; }
        public int Status { get; set; }
        public virtual Hospitals Hospitals { get; set; }
        public virtual Bloodbank Bloodbank { get; set; }
        public virtual Bloodtypes Bloodtypes { get; set; }
    }


}
