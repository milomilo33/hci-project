using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Zadatak
    {
        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public string Status { get; set; }

        public List<Ponuda> IzabranePonude { get; set; }

        public List<Komentar> Komentari { get; set; }
    }
}
