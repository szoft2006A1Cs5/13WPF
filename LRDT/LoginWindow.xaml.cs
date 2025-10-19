using LRDT.Database;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LRDT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LRDTContext Context { get; set; } = new LRDTContext();

        public LoginWindow()
        {
            InitializeComponent();

            foreach (var pincer in Context.Pincer)
            {
                var cbItem = new ComboBoxItem();
                cbItem.Tag = pincer.Id.ToString();
                cbItem.Content = pincer.Nev;
                cbUser.Items.Add(cbItem);
            }
        }

        private void cbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUser.SelectedItem != null)
            {
                btnLogin.IsEnabled = true;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (cbUser.SelectedItem == null) return;

            int pincerId = int.Parse((string)((ComboBoxItem)(cbUser.SelectedItem)).Tag);
            var pincer = Context.Pincer.Where(x => x.Id == pincerId).FirstOrDefault();

            if (pincer != null)
            {
                var rendelesWindow = new RendelesWindow(Context, pincer);
                rendelesWindow.Show();
                this.Close();
            }
        }
    }
}