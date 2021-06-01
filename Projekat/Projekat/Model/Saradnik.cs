using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Saradnik
    {
        public int Id { get; set; }

        public string Naziv { get; set; }

        public Adresa Adresa { get; set; }

        public string Opis { get; set; }

        public string Tip { get; set; }

        public string BrojTelefona { get; set; }

        public List<Dogadjaj> Dogadjaji { get; set; }

        public List<Ponuda> Ponude { get; set; }
    }
}
