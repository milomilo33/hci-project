using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Ponuda
    {
        private enum STATUS { PRIHVACENA, ODBIJENA, NACEKANJU}
        private int Id { get; set; }
        private double Cena { get; set; }
        private STATUS Status { get; set; }
    }
}
