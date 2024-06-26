﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Requests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Requestid { get; set; }
        [ForeignKey("Hospitals")]
        public int Hospitalid { get; set; }
        public string img { get; set; }
        public DateTime RequestDate { get; set; }
        public int quantity { get; set; }
        public string Contact { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public int status { get; set; }
        public virtual Hospitals Hospitals { get; set; }
        public virtual ICollection<Registers> Registers { get; set; } = new List<Registers>();
    }
}