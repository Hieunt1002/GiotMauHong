using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        [ForeignKey("Users")]
        public int Userid { get; set; }
        public string Content { get; set; }
        public string Datepost { get; set; }
        public int status { get; set;}
        public virtual Users Users { get; set; }   
    }
}
