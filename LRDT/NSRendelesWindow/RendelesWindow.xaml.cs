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
            //imgPincer.Source = SelectedPincer.KepAdat;

            foreach (var aktivRendeles in Context.Rendeles
                .Include(x => x.Asztal)
                .Include(x => x.Pincer)
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
            if (lbRendelesek.SelectedItem == null) return;

            var selRend = (RendelesListBoxView)lbRendelesek.SelectedItem;
            dgOrderItems.DataContext = selRend.Rendeles.RendelesTetels;
        }

        private void dgOrderItems_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (dgOrderItems.SelectedItem == null) return;

            var rt = ((RendelesTetel)dgOrderItems.SelectedItem);
            var mennyiseg = 0;

            if (!int.TryParse(((TextBox)(e.EditingElement)).Text, out mennyiseg))
                return;

            if (mennyiseg <= 0)
                rt.Rendeles.RendelesTetels.Remove(rt);
            else
                rt.Mennyiseg = mennyiseg;

            Context.SaveChanges();
            var selRend = (RendelesListBoxView)lbRendelesek.SelectedItem;
            dgOrderItems.DataContext = null; // Valamiert igenyli ezt a null-zast
            dgOrderItems.DataContext = selRend.Rendeles.RendelesTetels;

            lbRendelesek.Items.Refresh();
        }
    }
}
