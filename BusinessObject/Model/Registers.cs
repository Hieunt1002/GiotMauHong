using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Registers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegisterId { get; set; }
        [ForeignKey("Volunteers")]
        public int Volunteerid { get; set; }
        [ForeignKey("Requests")]
        public int Requestid { set; get; }
        public int Quantity { get; set; }
        [AllowNull]
        [ForeignKey("Bloodtypes")]
        public int Bloodtypeid { get; set; }
        public virtual Requests Requests { get; set; }
        public virtual Volunteers Volunteers { get; set; }
        public virtual Bloodtypes Bloodtypes { get; set; }
    }
}
