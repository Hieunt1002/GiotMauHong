using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class NumberBloodDTO
    {
        public int Bloodtypeid { get; set; }
        public string NameBlood { get; set; }
        public List<TotalBloodDTO> totalBloodDTOs { get; set; } = new List<TotalBloodDTO>();

    }
}
