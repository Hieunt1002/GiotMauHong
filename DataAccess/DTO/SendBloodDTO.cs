using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class SendBloodDTO
    {
        public int Hospitalid { get; set; }
        public int Bloodbankid { get; set;}
        public string Datesend { get; set; }
        public int Status { get; set; }
        public int numberbloodid { get; set; }
        public int SendBloodid { get; set; }
        public int Bloodtypeid { get; set; }
        public int quantity { get; set; }
    }
}
