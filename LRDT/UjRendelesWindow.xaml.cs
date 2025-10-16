using LRDT.Database;
using LRDT.Models;
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
    /// Interaction logic for UjRendelesWindow.xaml
    /// </summary>
    public partial class UjRendelesWindow : Window
    {
        LRDTContext Context { get; set; }
        public Asztal? ValasztottAsztal { get; set; }

        public UjRendelesWindow(LRDTContext ctx)
        {
            Context = ctx;
            ValasztottAsztal = null;

            InitializeComponent();

            var hasznaltAsztal = Context.Rendeles.Where(x => x.FizetesiMod == null).Select(x => x.AsztalId).ToList();

            foreach (var a in Context.Asztal.Where(x => !hasznaltAsztal.Contains(x.Id)))
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Tag = a;
                cbi.Content = $"{a.Id}";
                cbElerhetoAsztalok.Items.Add(cbi);
            }
        }

        private void cbElerhetoAsztalok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUj.IsEnabled = cbElerhetoAsztalok.SelectedItem != null;
        }

        private void btnVissza_Click(object sender, RoutedEventArgs e)
        {
            ValasztottAsztal = null;
            this.Close();
        }

        private void btnUj_Click(object sender, RoutedEventArgs e)
        {
            ValasztottAsztal = (Asztal)((ComboBoxItem)cbElerhetoAsztalok.SelectedItem).Tag;
            this.Close();
        }
    }
}
