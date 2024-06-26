﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Bloodtypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Bloodtypeid { get; set; }
        public string NameBlood { get; set; }
        public virtual ICollection<Registers> Registers { get; set; } = new List<Registers> ();
        public virtual ICollection<QuantityTake> QuantityTake { get; set; } = new List<QuantityTake>();
        public virtual ICollection<QuantitySend> QuantitySend { get; set; } = new List<QuantitySend>();

    }
}
