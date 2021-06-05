using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Dogadjaj
    {
        public int Id { get; set; }

        public double Budzet { get; set; }

        public int BrojGostiju { get; set; }

        public DateTime DatumOdrzavanja { get; set; }

        public string VrstaProslave { get; set; }

        public string Tema { get; set; }

        public string Napomena { get; set; }

        public string Status { get; set; }

        public bool BudzetPromenljiv { get; set; }

        public List<Zadatak> Zadaci { get; set; }

        public string DodatniZahtevi { get; set; }

        public Organizator Organizator { get; set; }

        public Predlog PrihvaceniGlavniPredlog { get; set; }

        public List<Predlog> PrihvaceniDodatniPredlozi { get; set; }

        public string MestoOdrzavanja { get; set; }
    }
}
