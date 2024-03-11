using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ViewRequest
    {
        public int Requestid { get; set; }
        public int Hospitalid { get; set; }
        public DateTime RequestDate { get; set; }
        public int quantity { get; set; }
        public string Contact { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public virtual Hospitals Hospitals { get; set; }
        public double total { get; set; }
        public int status { get; set; }
    }
}
