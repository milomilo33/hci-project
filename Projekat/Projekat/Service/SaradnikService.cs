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
	public class SaradnikService : ISaradnikService
	{
		private DatabaseContext context;

		public void UpdateSaradnik(Saradnik saradnik)
		{
			using (var db = new DatabaseContext())
			{
				var result = db.Saradnici.SingleOrDefault(s => s.Id == saradnik.Id);
				if (result != null)
				{
					result.Ponude = saradnik.Ponude;
					
				}
			}
		}

		public Saradnik getSaradnici(int id)
		{
			Saradnik saradnik = new Saradnik();

			saradnik = (Saradnik)(from s in context.Saradnici.Include("Ponude")
							  where s.Id.Equals(id)
							  select s);

			return saradnik;
		}

		public void RemoveSaradnik(int id)
		{
			var pg = context.Saradnici.First(s => s.Id == id);
			context.Saradnici.Remove(pg);
			context.SaveChanges();
		}

		public ObservableCollection<Saradnik> sviSaradnici()
		{
			ObservableCollection<Saradnik> saradnici = new ObservableCollection<Saradnik>();
			using (var db = new DatabaseContext())
			{
				saradnici = new ObservableCollection<Saradnik>(db.Saradnici.Include("Ponude"));
			}

			return saradnici;
		}
	}
}
