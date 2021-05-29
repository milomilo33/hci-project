using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Zadatak
    {
        private int Id { get; set; }
        private string Status { get; set; }
        private Dictionary<int,Ponuda> Ponude { get; set; }


    }
}
