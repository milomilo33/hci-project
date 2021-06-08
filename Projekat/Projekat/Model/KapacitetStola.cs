using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    [Serializable]
    public class KapacitetStola
    {
        [Key]
        public int? Id { get; set; }
        public string Naziv { get; set; }
        public int Kapacitet { get; set; }

        // koristice se samo u rasporedu sedenja
        public List<Gost> GostiZaStolom { get; set; }
    }
}
