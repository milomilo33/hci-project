using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class KapacitetStola
    {
        [Key]
        public string Naziv { get; set; }
        public int Kapacitet { get; set; }
    }
}
