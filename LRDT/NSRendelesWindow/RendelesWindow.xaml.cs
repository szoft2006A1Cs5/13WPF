using LRDT.Database;
using LRDT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LRDT.NSRendelesWindow;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZstdSharp.Unsafe;

namespace LRDT
{
    /// <summary>
    /// Interaction logic for RendelesWindow.xaml
    /// </summary>
    public partial class RendelesWindow : Window
    {
        public LRDTContext Context { get; set; }
        public Pincer SelectedPincer { get; set; }

        public RendelesWindow(LRDTContext ctx, Pincer pincer)
        {
            Context = ctx;
            SelectedPincer = pincer;

            InitializeComponent();
            
            this.Title = $"{this.SelectedPincer.Nev} - LRDT étteremkiszolgáló rendszer";
            TBPincer.Text = $"Szia {this.SelectedPincer.Nev}!";
            imgPincer.Source = this.SelectedPincer.KepAdat;

            foreach (var aktivRendeles in Context.Rendeles
                .Include(x => x.Asztal)
                .Include(x => x.Pincer)
                .Include(x => x.FizetesiMod)
                .Include(x => x.RendelesTetels)
                .ThenInclude(x => x.Tetel)
                .Where(x => x.Pincer.Id == SelectedPincer.Id &&
                            x.FizetesiMod == null
                )
            )
            {
                lbRendelesek.Items.Add(new RendelesListBoxItem
                    {
                        Rendeles = aktivRendeles
                    }
                );
            }
        }

        private void lbRendelesek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRendelesLezar.IsEnabled = lbRendelesek.SelectedItem != null;
            btnHozzaadTetel.IsEnabled = lbRendelesek.SelectedItem != null;

            if (lbRendelesek.SelectedItem == null) return;

            var selRend = (RendelesListBoxItem)lbRendelesek.SelectedItem;

            btnRendelesLezar.Content = selRend.Rendeles.RendelesTetels.Count() == 0 ? "Rendelés törlése" : "Rendelés lezárása";
            
            dgRendelesTetelek.DataContext = selRend.Rendeles.RendelesTetels;
        }

        private void dgRendelesTetelek_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dgRendelesTetelek.SelectedItem == null) return;

            var rt = ((RendelesTetel)dgRendelesTetelek.SelectedItem);
            var mennyiseg = 0;

            if (!int.TryParse(((TextBox)(e.EditingElement)).Text, out mennyiseg))
                return;

            if (mennyiseg <= 0)
                rt.Rendeles.RendelesTetels.Remove(rt);
            else
                rt.Mennyiseg = mennyiseg;

            Context.SaveChanges();
            var selRend = (RendelesListBoxItem)lbRendelesek.SelectedItem;
            dgRendelesTetelek.DataContext = null; // Valamiert igenyli ezt a null-zast
            dgRendelesTetelek.DataContext = selRend.Rendeles.RendelesTetels;
            btnRendelesLezar.Content = selRend.Rendeles.RendelesTetels.Count() == 0 ? "Rendelés törlése" : "Rendelés lezárása";

            lbRendelesek.Items.Refresh();
        }

        private void btnUjRendeles_Click(object sender, RoutedEventArgs e)
        {
            var ujrw = new UjRendelesWindow(Context);
            ujrw.ShowDialog();
            
            if (ujrw.ValasztottAsztal != null)
            {
                var rendeles = Context.Rendeles.Add(new Rendeles { 
                    Asztal = ujrw.ValasztottAsztal,
                    Datum = DateTime.Now,
                    Pincer = SelectedPincer,
                    RendelesTetels = new List<RendelesTetel>(),
                });

                Context.SaveChanges();

                var rendelesItem = new RendelesListBoxItem
                {
                    Rendeles = rendeles.Entity,
                };

                lbRendelesek.Items.Add(rendelesItem);

                lbRendelesek.SelectedItem = rendelesItem;
            }
        }

        private void btnRendelesLezar_Click(object sender, RoutedEventArgs e)
        {
            if (lbRendelesek.SelectedItem == null) return;

            var rlbi = (RendelesListBoxItem)lbRendelesek.SelectedItem;

            if (rlbi.Rendeles.RendelesTetels.Count() == 0)
            {
                lbRendelesek.SelectedItem = null;
                lbRendelesek.Items.Remove(rlbi);
                dgRendelesTetelek.DataContext = null;
                Context.Rendeles.Remove(rlbi.Rendeles);
                Context.SaveChanges();
                btnRendelesLezar.Content = "Rendelés lezárása";
                return;
            }

            var rlw = new RendelesLezarasWindow(Context, rlbi.Rendeles);
            rlw.ShowDialog();

            if (rlw.Rendeles.FizetesiMod != null)
            {
                Context.SaveChanges();
                lbRendelesek.SelectedItem = null;
                lbRendelesek.Items.Remove(rlbi);
                dgRendelesTetelek.DataContext = null;
            }
        }

        private void btnHozzaadTetel_Click(object sender, RoutedEventArgs e)
        {
            if (lbRendelesek.SelectedItem == null) return;

            var rlbi = (RendelesListBoxItem)lbRendelesek.SelectedItem;
            var thw = new TetelHozzaadWindow(Context, rlbi.Rendeles);
            thw.ShowDialog();

            dgRendelesTetelek.Items.Refresh();
            lbRendelesek.Items.Refresh();

            btnRendelesLezar.Content = rlbi.Rendeles.RendelesTetels.Count() == 0 ? "Rendelés törlése" : "Rendelés lezárása";
        }

        private void btnEltavolitTetel_Click(object sender, RoutedEventArgs e)
        {
            if (dgRendelesTetelek.SelectedItem == null) return;

            var rt = (RendelesTetel)dgRendelesTetelek.SelectedItem;
            dgRendelesTetelek.SelectedItem = null;

            var rendeles = rt.Rendeles;

            Context.RendelesTetel.Remove(rt);
            Context.SaveChanges();

            dgRendelesTetelek.Items.Refresh();
            lbRendelesek.Items.Refresh();

            btnRendelesLezar.Content = rendeles.RendelesTetels.Count() == 0 ? "Rendelés törlése" : "Rendelés lezárása";
        }

        private void dgRendelesTetelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEltavolitTetel.IsEnabled = dgRendelesTetelek.SelectedItem != null;
        }

        private void btnLezartRendelesek_Click(object sender, RoutedEventArgs e)
        {
            var lrw = new LezartRendelesWindow(Context);
            lrw.ShowDialog();
        }

        private void btnTetelModositas_Click(object sender, RoutedEventArgs e)
        {
            var tmw = new TetelModositasWindow(Context);
            tmw.ShowDialog();
        }

        private void btnStatisztikak_Click(object sender, RoutedEventArgs e)
        {
            var sw = new StatisztikaWindow(Context);
            sw.ShowDialog();
        }
    }
}
