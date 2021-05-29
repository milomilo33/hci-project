using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Dogadjaj
    {
        private int Id { get; set; }
        private double Budzet { get; set; }
        private DateTime DatumOdrzavanja { get; set; }
        private string VrstaProslave { get; set; }
        private string Tema { get; set; }
        private string Napomena { get; set; }
        private string Status { get; set; }
        private bool BudzetPromenljiv { get; set; }
        private Dictionary<int,Zadatak> Zadaci { get; set; }



    }
}
