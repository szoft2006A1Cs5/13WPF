using LRDT.Database;
using LRDT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for RendelesLezarasWindow.xaml
    /// </summary>
    public partial class RendelesLezarasWindow : Window
    {
        public LRDTContext Context { get; set; }
        public Rendeles Rendeles { get; set; }

        public RendelesLezarasWindow(LRDTContext ctx, Rendeles rendeles)
        {
            Context = ctx;
            Rendeles = rendeles;

            InitializeComponent();

            foreach (var fm in Context.FizetesiMod)
            {
                var cbi = new ComboBoxItem();
                cbi.Tag = fm;
                cbi.Content = fm.Nev;
                cbFizetesiMod.Items.Add(cbi);
            }

            string osszesites = "Összesítve\n";

            foreach (var tetel in rendeles.RendelesTetels)
            {
                osszesites += $"- {tetel.Tetel.Nev} - {tetel.Tetel.Ar} Ft * {tetel.Mennyiseg} db - {tetel.OsszAr} Ft\n";
            }

            TBElszamolas.Text = osszesites;

            TBVegosszeg.Text = $"{rendeles.RendelesTetels.Sum(x => x.OsszAr)} Ft";
        }

        private void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbFizetesiMod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnLezaras.IsEnabled = cbFizetesiMod.SelectedItem != null;
        }

        private void btnLezaras_Click(object sender, RoutedEventArgs e)
        {
            Rendeles.FizetesiMod = (FizetesiMod)((ComboBoxItem)cbFizetesiMod.SelectedItem).Tag;
            this.Close();
        }
    }
}
