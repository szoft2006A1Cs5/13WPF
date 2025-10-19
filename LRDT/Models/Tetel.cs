using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LRDT.Models
{
    public class Tetel
    {
        public int Id { get; set; }
        public string? Nev { get; set; }
        public int Ar { get; set; }
        public bool Elerheto { get; set; }

        [NotMapped]
        public BitmapImage? KepAdat { get; set; }
        [NotMapped]
        private string? kepSrc;
        public string? Kep { 
            get { 
                return kepSrc;
            } 
            set {
                kepSrc = value;
                this.KepAdat = new BitmapImage(new Uri(kepSrc, UriKind.RelativeOrAbsolute));
            } 
        }

        public bool GyerekMenu { get; set; }
        public string? Allergen { get; set; }

    }
}
