using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model  
{
    public class QuantitySend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int quantitysendid { get; set; }
        [ForeignKey("NumberBlood")]
        public int numberbloodid { get; set; }
        [ForeignKey("SendBlood")]
        public int SendBloodid { get; set; }
        [ForeignKey("Bloodtypes")]
        public int Bloodtypeid { get; set; }
        public int quantity { get; set; }
        public virtual SendBlood SendBlood { get; set; }
        public virtual NumberBlood NumberBlood { get; set; }
        public virtual Bloodtypes Bloodtypes { get; set; }
    }
}
