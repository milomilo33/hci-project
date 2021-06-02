using Projekat.Data;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
	class PonudaService : IPonudaService
	{
		private static DatabaseContext context = new DatabaseContext();
		public Ponuda getPonuda(int id)
		{
			Ponuda ponuda = new Ponuda();

			ponuda = (Ponuda)(from p in context.Ponude
					 where p.Id.Equals(id)
					 select p);

			return ponuda;
		}

		public ObservableCollection<Ponuda> svePonude()
		{
			ObservableCollection<Ponuda> ponude = new ObservableCollection<Ponuda>();
			using (var db = new DatabaseContext())
			{
				ponude = new ObservableCollection<Ponuda>(db.Ponude);
			}

			return ponude;
		}
	}
}
