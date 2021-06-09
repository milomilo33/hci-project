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
    public class KomentarService : IKomentarService
    {
        public ObservableCollection<Komentar> getKomentareZaZadatak(int id)
        {
           
            ObservableCollection<Komentar> komentari = new ObservableCollection<Komentar>();

            using (var db = new DatabaseContext())
            {
                  komentari = new ObservableCollection<Komentar> (db.Zadaci.Include("Komentari").Include("Komentari.Autor").SingleOrDefault(z => z.Id == id).Komentari.OrderBy(z=>z.DatumVremeKomentara));
                
            }
            return komentari;
            
        }

        public ObservableCollection<Komentar> getKomentareZaDogadjaj(int id)
        {

            ObservableCollection<Komentar> komentari = new ObservableCollection<Komentar>();

            using (var db = new DatabaseContext())
            {
                komentari = new ObservableCollection<Komentar>(db.Dogadjaji.Include("Komentari").Include("Komentari.Autor").SingleOrDefault(d => d.Id == id).Komentari.OrderBy(z => z.DatumVremeKomentara));
            }
            return komentari;

        }
    }
}
