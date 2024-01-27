using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Activate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Activateid { get; set; }
        public string NameActivate { get; set; }
        public DateTime Datepost { get; set; }
        public virtual ICollection<Images> Images { get; set; } = new List<Images>();
    }
}
