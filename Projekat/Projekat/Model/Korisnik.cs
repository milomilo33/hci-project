using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public abstract class Korisnik
    {
            private string Email { get; set; }

            private string Lozinka { get; set; }

            private string Ime { get; set; }
            private string Prezime { get; set; }
            private string BrojTelefona { get; set; }

            private Adresa Adresa { get; set; }

    }

    public class Organizator :Korisnik
    {
       Dictionary<int,Dogadjaj> Dogadjaji { get; set; }
    }

    public class Klijent: Korisnik
    {
        Dictionary<int, Dogadjaj> Dogadjaji { get; set; }
    }
    public class Administrator:Korisnik
    {

    }
}
