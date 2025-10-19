using LRDT.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LRDT
{
    /// <summary>
    /// Interaction logic for StatisztikaWindow.xaml
    /// </summary>
    public partial class StatisztikaWindow : Window
    {
        LRDTContext Context { get; set; }
        public StatisztikaWindow(LRDTContext ctx)
        {
            Context = ctx;

            InitializeComponent();

            // Legtobb rendelessel rendelkezo felszolgalok
            var legtPincerRendeles = Context.Rendeles.Include(x => x.Pincer).GroupBy(x => x.Pincer).Select(x => x.Count()).Max();
            var legtRendPincerek = Context.Rendeles
                .Include(x => x.Pincer)
                .GroupBy(x => x.Pincer)
                .Where(x => x.Count() == legtPincerRendeles)
                .Select(x => x.Key.Nev);

            TBLegtRendFelsz.Text = $"{string.Join(", ", legtRendPincerek)} ({legtPincerRendeles}x)";

            // Legtobb rendelessel rendelkezo asztal
            var legtAsztalRendeles = Context.Rendeles
                .GroupBy(x => x.AsztalId)
                .Select(x => x.Count())
                .Max();
            var legtRendAsztalok = Context.Rendeles
                .GroupBy(x => x.AsztalId)
                .Where(x => x.Count() == legtAsztalRendeles)
                .Select(x => $"{x.Key}. asztal");

            TBLegtRendAsztal.Text = $"{string.Join(", ", legtRendAsztalok)} ({legtAsztalRendeles}x)";

            // Legtobbet rendelt tetel
            var legtRendelesTetelenkent = Context.RendelesTetel
                .Include(x => x.Tetel)
                .GroupBy(x => x.Tetel)
                .Select(x => new { Tetel = x.Key, RendelesekSzama = x.Sum(x => x.Mennyiseg) });

            var legtRendeltTetelek = legtRendelesTetelenkent
                .Where(x => x.RendelesekSzama == legtRendelesTetelenkent.Max(x => x.RendelesekSzama))
                .Select(x => x.Tetel.Nev);

            TBLegtRendTetel.Text = $"{string.Join(", ", legtRendeltTetelek)} ({legtRendelesTetelenkent.Max(x => x.RendelesekSzama)}x)";

            // Atlag koltes
            var atlagKoltes = Context.Rendeles
                .Include(x => x.RendelesTetels)
                .ThenInclude(x => x.Tetel)
                .ToList()
                .Average(x => x.RendelesTetels.Sum(y => y.OsszAr));

            TBAtlagKoltes.Text = $"{atlagKoltes:F2} Ft";

            // Atlag vendegek szama
            var atlagVendeg = Context.Rendeles
                .Include(x => x.Asztal)
                .Average(x => x.Asztal.Ferohely);

            TBAtlagVendeg.Text = $"{atlagVendeg:F2} vendég";
        }
    }
}
