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
    public class ZadatakService : IZadatakService
    {

        public ObservableCollection<Zadatak> sviZadaciZaDogadjaj(int id)
        {
            ObservableCollection<Zadatak> zadaci = new ObservableCollection<Zadatak>();
            using (var db = new DatabaseContext())
            {
                zadaci = new ObservableCollection<Zadatak>(db.Zadaci.Include("Dogadjaj").Where(d=>d.Dogadjaj.Id.Equals(id)));
            }

            return zadaci;
        }
    }
}
