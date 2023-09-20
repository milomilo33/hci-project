using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    [Serializable]
    public class Gost
    {
        [Key]
        public int? Id { get; set; }

        public string ImeIPrezime { get; set; }
    }
}
