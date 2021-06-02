using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
	public class Predlog
	{
		public enum STATUS { PRIHVACEN, ODBIJEN, NA_CEKANJU }

		public int Id { get; set; }

		public Ponuda Ponuda { get; set;}

		public Zadatak Zadatak { get; set; }

		public STATUS Status { get; set; }

		
	}
}
