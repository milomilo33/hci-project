using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Saradnik
    {
        private string Naziv { get; set; }
        private Adresa Adresa { get; set; }
        private string Tip { get; set; }
        private string BrojTelefona { get; set; }

        private Dictionary<int,Dogadjaj> Dogadjaji { get; set; }


    }
}
