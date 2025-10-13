using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.Models
{
    public class RendelesTetel
    {
        public int TetelId { get; set; }
        public Tetel Tetel { get; set; }
        public int RendelesId { get; set; }
        public Rendeles Rendeles { get; set; }
        public int Mennyiseg { get; set; }
    }
}
