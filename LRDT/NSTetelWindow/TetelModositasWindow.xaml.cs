using LRDT.Database;
using LRDT.Models;
using LRDT.NSTetelWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for TetelModositasWindow.xaml
    /// 
    /// Teteleket modosithatjuk, viszont mivel a kepeket a binary-ba
    /// epitjuk be Resource-kent, nem az adatbazisbol toltjuk be,
    /// azokat nem tudnank se modositani, se hozzaadni, ezert a Teteleket
    /// csak modosithatjuk.
    /// Torolni nem lehetne egyszeruen, mivel lehetnek olyan regi rendelesek,
    /// amelyek fuggenek a teteltol.
    /// </summary>
    public partial class TetelModositasWindow : Window
    {
        LRDTContext Context { get; set; }

        public TetelModositasWindow(LRDTContext ctx)
        {
            Context = ctx;

            InitializeComponent();

            genListBoxItems();
        }

        private void genListBoxItems()
        {
            lbTetelek.SelectedItem = null;
            lbTetelek.Items.Clear();

            foreach (var tetel in Context.Tetel.Where(x => x.Nev!.Contains(tbKereses.Text) && 
                                (chbKeresesGyerek.IsChecked == true ? (x.GyerekMenu == true) : true)))
            {
                lbTetelek.Items.Add(new TetelListBoxItem
                {
                    Tetel = tetel,
                });
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lbTetelek.SelectedItem == null) return;

            var tetel = ((TetelListBoxItem)lbTetelek.SelectedItem)!.Tetel;

            var ujAr = 0;

            if (!int.TryParse(tbAr.Text, out ujAr))
            {
                MessageBox.Show("Árként csak egész számokat adhatsz meg!", "Hiba");
                return;
            }

            var mbRes = MessageBox.Show($"A mentéssel az előző rendelések árai is változnak!", "Biztosan mented?", MessageBoxButton.YesNo);
            if (mbRes == MessageBoxResult.No) return;

            tetel.Nev = tbNev.Text;
            tetel.Ar = ujAr;
            tetel.Allergen = tbAllergenek.Text;
            tetel.Elerheto = chbElerheto.IsChecked!.Value;
            tetel.GyerekMenu = chbGyerek.IsChecked!.Value;

            Context.SaveChanges();

            lbTetelek.Items.Refresh();
        }

        private void chbKeresesGyerek_Changed(object sender, RoutedEventArgs e)
        {
            genListBoxItems();
        }

        private void tbKereses_TextChanged(object sender, TextChangedEventArgs e)
        {
            genListBoxItems();
        }

        private void lbTetelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool vanTetel = lbTetelek.SelectedItem != null;
            tbNev.IsEnabled = vanTetel;
            tbAr.IsEnabled = vanTetel;
            tbAllergenek.IsEnabled = vanTetel;
            chbElerheto.IsEnabled = vanTetel;
            chbGyerek.IsEnabled = vanTetel;
            btnSave.IsEnabled = vanTetel;

            var tlbi = (TetelListBoxItem)lbTetelek.SelectedItem!;

            tbNev.Text = vanTetel ? tlbi.Tetel.Nev : "";
            tbAr.Text = vanTetel ? $"{tlbi.Tetel.Ar}" : "";
            tbAllergenek.Text = vanTetel ? tlbi.Tetel.Allergen : "";
            chbElerheto.IsChecked = vanTetel ? tlbi.Tetel.Elerheto : false;
            chbGyerek.IsChecked = vanTetel ? tlbi.Tetel.GyerekMenu : false;
        }
    }
}
