using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class NumberBlood
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int numberbloodid { get; set; }
        public int quantity { get; set; }
        public virtual ICollection<QuantitySend> QuantitySends { get; set; } = new List<QuantitySend>();
        public virtual ICollection<QuantityTake> QuantityTake { get; set; } = new List<QuantityTake>();

    }
}
