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
        public Zadatak getZadatak(int id)
        {
            Zadatak zadatak = new Zadatak();
            using (var db = new DatabaseContext())
            {
                zadatak = db.Zadaci.Find(id);
            }

            return zadatak;
        }

        public Zadatak getZadatakSaPredlogom(int id)
        {
            Zadatak zadatak = new Zadatak();
            using (var db = new DatabaseContext())
            {
                zadatak = db.Zadaci.Include("IzabraniPredlog").Where(d => d.Id.Equals(id)).FirstOrDefault();
            }

            return zadatak;

        }

        public bool proveraDaLiPostoji(int id)
        {
          
            bool value = false;
            ObservableCollection<Zadatak> zadaci = new ObservableCollection<Zadatak>();
            using (var db = new DatabaseContext())
            {
                zadaci = new ObservableCollection<Zadatak>(db.Zadaci.Include("Dogadjaj").Where(d => d.Dogadjaj.Id.Equals(id)));
            }
            if (zadaci.Count == 0)
            {
                value = false;
                return value;
            }
            foreach (Zadatak z in zadaci)
            {
                if (z.Tip == Zadatak.TipZadatka.GLAVNI)
                {
                    
                    value = true;

                }
            }
            return value;
        }

        public bool proveraGlavniZadatak(int id)
        {
            bool value = true;
            ObservableCollection<Zadatak> zadaci = new ObservableCollection<Zadatak>();
            using (var db = new DatabaseContext())
            {
                zadaci = new ObservableCollection<Zadatak>(db.Zadaci.Include("Dogadjaj").Include("IzabraniPredlog").Where(d => d.Dogadjaj.Id.Equals(id)));
            }
            if(zadaci.Count == 0)
            {
                value = false;
                return value;
            }
            foreach(Zadatak z in zadaci){
                //if(z.Tip == Zadatak.TipZadatka.GLAVNI)
                //{
                    if(z.IzabraniPredlog == null)
                    {
                        value = false;
                    }
                   

                //}
            }
            return value;
        }

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
