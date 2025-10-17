using LRDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.NSRendelesWindow
{
    public class RendelesListBoxItem
    {
        public Rendeles Rendeles { get; set; }

        public override string ToString()
        {
            return Rendeles.RendelesTetels != null ? $"{Rendeles.Asztal.Id}. asztal - {Rendeles.RendelesTetels.Sum(x => x.Mennyiseg * x.Tetel.Ar)} Ft" : $"{Rendeles.AsztalId}. asztal";
        }
    }
}
