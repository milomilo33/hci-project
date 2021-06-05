using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Zadatak
    {
        public enum TipZadatka { GLAVNI, DODATNI }

        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public string Status { get; set; }

        // public List<Predlog> IzabraniPredlozi { get; set; }

        public Predlog IzabraniPredlog { get; set; }

        public List<Komentar> Komentari { get; set; }

        public TipZadatka Tip { get; set; }
    }
}
