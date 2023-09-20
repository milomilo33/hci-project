using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Ponuda
    {

        public int Id { get; set; }

        public double Cena { get; set; }

        public string Opis { get; set; }

        public Saradnik Saradnik { get; set; }

        public bool Nevazeca { get; set; }

        // treba dodati slike za ponude
    }
}
