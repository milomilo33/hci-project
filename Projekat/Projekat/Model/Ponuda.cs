using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Ponuda
    {
        public enum STATUS { PRIHVACENA, ODBIJENA, NA_CEKANJU }

        public int Id { get; set; }

        public double Cena { get; set; }

        public STATUS Status { get; set; }

        public string Opis { get; set; }

        // treba dodati slike za ponude
    }
}
