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
            
            this.Title = $"{this.SelectedPincer.Nev} - LRDT";

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
                lbRendelesek.Items.Add(new RendelesListBoxView
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

            var selRend = (RendelesListBoxView)lbRendelesek.SelectedItem;
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
            var selRend = (RendelesListBoxView)lbRendelesek.SelectedItem;
            dgRendelesTetelek.DataContext = null; // Valamiert igenyli ezt a null-zast
            dgRendelesTetelek.DataContext = selRend.Rendeles.RendelesTetels;

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

                var rendelesView = new RendelesListBoxView
                {
                    Rendeles = rendeles.Entity,
                };

                lbRendelesek.Items.Add(rendelesView);

                lbRendelesek.SelectedItem = rendelesView;
            }
        }

        private void btnRendelesLezar_Click(object sender, RoutedEventArgs e)
        {
            if (lbRendelesek.SelectedItem == null) return;

            var rlbv = (RendelesListBoxView)lbRendelesek.SelectedItem;

            var rlw = new RendelesLezarasWindow(Context, rlbv.Rendeles);
            rlw.ShowDialog();

            if (rlw.Rendeles.FizetesiMod != null)
            {
                Context.SaveChanges();
                lbRendelesek.SelectedItem = null;
                lbRendelesek.Items.Remove(rlbv);
                dgRendelesTetelek.DataContext = null;
            }
        }

        private void btnHozzaadTetel_Click(object sender, RoutedEventArgs e)
        {
            if (lbRendelesek.SelectedItem == null) return;

            var rlbv = (RendelesListBoxView)lbRendelesek.SelectedItem;
            var thw = new TetelHozzaadWindow(Context, rlbv.Rendeles);
            thw.ShowDialog();

            dgRendelesTetelek.Items.Refresh();
            lbRendelesek.Items.Refresh();
        }

        private void btnEltavolitTetel_Click(object sender, RoutedEventArgs e)
        {
            if (dgRendelesTetelek.SelectedItem == null) return;

            var rt = (RendelesTetel)dgRendelesTetelek.SelectedItem;
            dgRendelesTetelek.SelectedItem = null;

            Context.RendelesTetel.Remove(rt);
            Context.SaveChanges();

            dgRendelesTetelek.Items.Refresh();
            lbRendelesek.Items.Refresh();
        }

        private void dgRendelesTetelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEltavolitTetel.IsEnabled = dgRendelesTetelek.SelectedItem != null;
        }
    }
}
