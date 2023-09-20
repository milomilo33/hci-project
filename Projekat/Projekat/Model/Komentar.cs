using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Komentar
    {
        public int Id { get; set; }

        public DateTime DatumVremeKomentara { get; set; }

        public Korisnik Autor { get; set; }

        public string Sadrzaj { get; set; }
    }
}
