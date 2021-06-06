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

		public Ponuda AddPonuda(Ponuda ponuda)
		{
			Ponuda sacuvana = context.Ponude.Add(ponuda);
			context.SaveChanges();

			return sacuvana;

		}

		public Ponuda getPonuda(int id)
		{
			Ponuda ponuda = new Ponuda();

			ponuda = (Ponuda)(from p in context.Ponude.Include("Saradnik")
							  where p.Id.Equals(id) && p.Nevazeca.Equals(false)
							  select p) ;
			
			return ponuda;
		}

		public void RemovePonuda(int id)
		{
			var pg = context.Ponude.First(p => p.Id == id);
			pg.Nevazeca = true;
			context.SaveChanges();
		}

		public ObservableCollection<Ponuda> svePonude()
		{
			ObservableCollection<Ponuda> ponude = new ObservableCollection<Ponuda>();
			using (var db = new DatabaseContext())
			{
				ponude = new ObservableCollection<Ponuda>(db.Ponude.Include("Saradnik"));
			}

			return ponude;
		}
	}
}
