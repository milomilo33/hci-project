using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public abstract class Korisnik
    {
        [Key]
        public string Email { get; set; }

        public string Lozinka { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string BrojTelefona { get; set; }

        public Adresa Adresa { get; set; }
    }

    public class Organizator : Korisnik
    {
        public List<Dogadjaj> Dogadjaji { get; set; }
    }

    public class Klijent : Korisnik
    {
        public List<Dogadjaj> Dogadjaji { get; set; }
    }
    public class Administrator : Korisnik
    {

    }
}
