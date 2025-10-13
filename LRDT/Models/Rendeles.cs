using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRDT.Models
{
    public class Rendeles
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string? FizetesiMod { get; set; }
        public int AsztalId { get; set; }
        public Asztal Asztal { get; set; }
        public int PincerId { get; set; }
        public Pincer Pincer { get; set; }

        public List<RendelesTetel> RendelesTetels { get; set; }
    }
}
