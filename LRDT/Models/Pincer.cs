using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LRDT.Models
{
    public class Pincer
    {
        public int Id { get; set; }
        public string Nev { get; set; }

        [NotMapped]
        public BitmapImage? KepAdat { get; set; }
        [NotMapped]
        private string? kepSrc;
        public string? Kep
        {
            get
            {
                return kepSrc;
            }
            set
            {
                kepSrc = value;
                this.KepAdat = new BitmapImage(new Uri(kepSrc, UriKind.Relative));
            }
        }
    }
}
