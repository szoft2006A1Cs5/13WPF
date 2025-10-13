using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.Models
{
    public class Tetel
    {
        public int Id { get; set; }
        public string? Nev { get; set; }
        public int Ar { get; set; }
        public bool Elerheto { get; set; }
        public string? Kep { get; set; }
        public bool GyerekFelnott { get; set; }
        public string? Allergen { get; set; }

    }
}
