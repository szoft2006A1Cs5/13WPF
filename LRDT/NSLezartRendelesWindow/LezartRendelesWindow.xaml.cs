using LRDT.Database;
using LRDT.Models;
using LRDT.NSLezartRendelesWindow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for LezartRendelesWindow.xaml
    /// </summary>
    public partial class LezartRendelesWindow : Window
    {
        private LRDTContext Context { get; set; }

        public LezartRendelesWindow(LRDTContext ctx)
        {
            Context = ctx;

            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            Thread.CurrentThread.CurrentCulture = ci;

            TBAsztalszam.Text = $"Asztalszám ({Context.Asztal.Min(x => x.Id)}-{Context.Asztal.Max(x => x.Id)})";

            generateLBItems();
        }

        private void generateLBItems()
        {
            lbLezartRendelesek.SelectedItem = null;
            lbLezartRendelesek.Items.Clear();

            var validAsztalok = Context.Asztal.Select(x => x.Id);
            IEnumerable<Rendeles> rendelesek = Context.Rendeles
                                                .Include(x => x.FizetesiMod)
                                                .Include(x => x.Asztal)
                                                .Include(x => x.RendelesTetels)
                                                .ThenInclude(x => x.Tetel)
                                                .Where(x => x.FizetesiMod != null);

            int asztalId = 0;
            int.TryParse(tbAsztalszam.Text, out asztalId);

            if (validAsztalok.Contains(asztalId))
                rendelesek = rendelesek.Where(x => x.AsztalId == asztalId);

            if (dpRendelesDatum.SelectedDate != null)
                rendelesek = rendelesek.Where(x => x.Datum.Date == dpRendelesDatum.SelectedDate);

            foreach (var rendeles in rendelesek)
            {
                lbLezartRendelesek.Items.Add(new NSLezartRendelesWindow.RendelesListBoxItem
                {
                    Rendeles = rendeles,
                });
            }
        }

        private void dpRendelesDatum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            generateLBItems();
        }

        private void tbAsztalszam_TextChanged(object sender, TextChangedEventArgs e)
        {
            generateLBItems();
        }

        private void lbLezartRendelesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBAsztal.Text = "";
            TBDatum.Text = "";
            TBFelszolgalo.Text = "";
            TBFizMod.Text = "";
            TBVegosszeg.Text = "";
            dgRendelesTetelek.DataContext = null;

            if (lbLezartRendelesek.SelectedItem == null) return;

            var rlbi = (RendelesListBoxItem)lbLezartRendelesek.SelectedItem;

            TBAsztal.Text = $"{rlbi.Rendeles.AsztalId}. asztal";
            TBDatum.Text = rlbi.Rendeles.Datum.ToString("yyyy/MM/dd");
            TBFelszolgalo.Text = rlbi.Rendeles.Pincer.Nev;
            TBFizMod.Text = rlbi.Rendeles.FizetesiMod!.Nev;
            TBVegosszeg.Text = $"{rlbi.Rendeles.RendelesTetels.Sum(x => x.OsszAr)} Ft";
            dgRendelesTetelek.DataContext = rlbi.Rendeles.RendelesTetels;
        }
    }
}
