using LRDT.Database;
using LRDT.Models;
using LRDT.NSTetelWindow;
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
    /// Interaction logic for TetelHozzaadWindow.xaml
    /// </summary>
    public partial class TetelHozzaadWindow : Window
    {
        public LRDTContext Context { get; set; }
        public Rendeles Rendeles { get; set; }

        public TetelHozzaadWindow(LRDTContext ctx, Rendeles rendeles)
        {
            Context = ctx;
            Rendeles = rendeles;

            InitializeComponent();

            generateLBItems();
        }

        private void generateLBItems()
        {
            lbElerhetoTetelek.SelectedItem = null;
            lbElerhetoTetelek.Items.Clear();
            
            foreach (var tetel in Context.Tetel.Where(x => !Rendeles.RendelesTetels.Select(y => y.Tetel).Contains(x) &&
                                x.Elerheto == true &&
                                x.Nev!.Contains(tbKereses.Text) &&
                                (chbGyerek.IsChecked == true ? (x.GyerekMenu == true) : true)))
            {
                lbElerhetoTetelek.Items.Add(new TetelListBoxItem
                {
                    Tetel = tetel,
                });
            }
        }

        private void tbMennyiseg_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBTeljesar.Text = "";

            int mennyiseg = 0;
            if (!int.TryParse(tbMennyiseg.Text, out mennyiseg))
            {
                btnHozzaad.IsEnabled = false;
                return;
            }

            if (mennyiseg <= 0) return;

            if (lbElerhetoTetelek.SelectedItem == null) return;

            var tlbi = (TetelListBoxItem)lbElerhetoTetelek.SelectedItem;
            TBTeljesar.Text = $"{tlbi.Tetel.Ar * mennyiseg} Ft";
            btnHozzaad.IsEnabled = true;
        }

        private void tbKereses_TextChanged(object sender, TextChangedEventArgs e)
        {
            generateLBItems();
        }

        private void chbGyerek_Changed(object sender, RoutedEventArgs e)
        {
            generateLBItems();
        }

        private void lbElerhetoTetelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBTeljesar.Text = "";
            TBEgysegar.Text = "";
            TBAllergenek.Text = "";
            imgEtel.Source = null;

            if (lbElerhetoTetelek.SelectedItem == null) return;

            var tlbi = (TetelListBoxItem)lbElerhetoTetelek.SelectedItem;

            TBEgysegar.Text = $"{tlbi.Tetel.Ar} Ft";
            TBAllergenek.Text = tlbi.Tetel.Allergen;

            tbMennyiseg_TextChanged(null, null);
            
            imgEtel.Source = tlbi.Tetel.KepAdat;
        }

        private void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnHozzaad_Click(object sender, RoutedEventArgs e)
        {
            if (lbElerhetoTetelek.SelectedItem == null) return;
            int mennyiseg = 0;
            if (!int.TryParse(tbMennyiseg.Text, out mennyiseg))
            {
                btnHozzaad.IsEnabled = false;
                return;
            }

            if (mennyiseg <= 0) return;

            var tlbi = (TetelListBoxItem)lbElerhetoTetelek.SelectedItem;

            Context.RendelesTetel.Add(new RendelesTetel
            {
                Tetel = tlbi.Tetel,
                Mennyiseg = mennyiseg,
                Rendeles = Rendeles,
            });

            Context.SaveChanges();
            this.Close();
        }
    }
}
