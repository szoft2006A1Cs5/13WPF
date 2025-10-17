using LRDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.NSLezartRendelesWindow
{
    class RendelesListBoxItem
    {
        public Rendeles Rendeles { get; set; }

        public override string ToString()
        {
            return $"{Rendeles.AsztalId}. asztal - {Rendeles.Datum.ToString("yyyy/MM/dd")} - {Rendeles.RendelesTetels.Sum(x => x.OsszAr)} Ft";
        }
    }
}
