using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class KomunikacijaViewModel:ViewModelBase
    {
        public ObservableCollection<Komentar> _komentari;
        private readonly IKomentarService KomentarService = new KomentarService();
        public int _idZadatka;
        private ICommand _posaljiCommand;
        private string _sadrzaj;
        private Korisnik k;
        public ObservableCollection<Komentar> Komentari
        {
            get { return _komentari; }
            set
            {
                _komentari = value;
                OnPropertyChanged(nameof(Komentari));
            }
        }

        public int IdZadatka
        {
            get => _idZadatka;
            set
            {
                _idZadatka = value;
                OnPropertyChanged(nameof(IdZadatka));
            }
        }

        public string Sadrzaj
        {
            get => _sadrzaj;
            set
            {
                _sadrzaj = value;
                OnPropertyChanged(nameof(Sadrzaj));
            }
        }

        public ICommand PosaljiCommand
        {
            get
            {
                if (_posaljiCommand == null)
                    _posaljiCommand = new RelayCommand(_posaljiCommand => Posalji());
                return _posaljiCommand;
            }
        }

        public void Posalji()
        {
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            k = korisnikStore.TrenutniKorisnik;
            using (var db = new DatabaseContext())
            {
                var z = db.Zadaci.Include("Komentari").SingleOrDefault(zad => zad.Id == IdZadatka);
                var autor = db.Korisnici.SingleOrDefault(s => s.Email == k.Email);
                Komentar komentar = new Komentar();
                komentar.DatumVremeKomentara = DateTime.Now;
                komentar.Sadrzaj = Sadrzaj;
                komentar.Autor = autor;
                z.Komentari.Add(komentar);
                db.Komentari.Add(komentar);
                db.SaveChanges();
            }
            Sadrzaj = "";
            loadKomentari();
        }
        public void loadKomentari()
        {
            Komentari = KomentarService.getKomentareZaZadatak(IdZadatka);
           

        }
        public KomunikacijaViewModel()
        {
           

        }
    }
}
