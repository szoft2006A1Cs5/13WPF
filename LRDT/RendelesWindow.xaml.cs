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
            
            this.Title = this.SelectedPincer.Nev;
        }
    }
}
