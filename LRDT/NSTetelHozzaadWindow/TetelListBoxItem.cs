using LRDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.NSTetelHozzaadWindow
{
    class TetelListBoxItem
    {
        public Tetel Tetel { get; set; }

        public override string ToString()
        {
            return $"{Tetel.Nev}";
        }
    }
}
