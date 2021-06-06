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
	public class DogadjajService : IDogadjajService
	{
		private static DatabaseContext context = new DatabaseContext();

		public Dogadjaj getDogadjaj(int id)
		{
			Dogadjaj dogadjaj = new Dogadjaj();

			dogadjaj = (Dogadjaj)(from d in context.Dogadjaji.Include("Organizator")
								  where d.Id.Equals(id)
								  select d).FirstOrDefault();


			return dogadjaj;
		}

		public ObservableCollection<Dogadjaj> sviDogadjaji()
		{
			ObservableCollection<Dogadjaj> dogadjaji = new ObservableCollection<Dogadjaj>();
			using (var db = new DatabaseContext())
			{
				dogadjaji = new ObservableCollection<Dogadjaj>(db.Dogadjaji.Include("Organizator"));
			}

			return dogadjaji;

		}

		public ObservableCollection<Dogadjaj> sviDogadjajiZaOrganizatora(String email)
		{

			ObservableCollection<Dogadjaj> dogadjaji = new ObservableCollection<Dogadjaj>();
			using (var db = new DatabaseContext())
			{
				dogadjaji = new ObservableCollection<Dogadjaj>(db.Dogadjaji.Include("Organizator").Where(d=>d.Organizator.Email.Equals(email)));
			}

			return dogadjaji;

		}

		public void updateStatus(Dogadjaj dogadjaj)
		{
			using (var db = new DatabaseContext())
			{

				var dog = db.Dogadjaji.Find(dogadjaj.Id);
				dog.Status = dogadjaj.Status;
				db.SaveChanges();

			}
		}

	}
}
