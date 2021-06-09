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
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class KomunikacijaViewModel : ViewModelBase
    {
        public ObservableCollection<Komentar> _komentari;
        private readonly IKomentarService KomentarService = new KomentarService();
        public int _idZadatka;
        private ICommand _posaljiCommand;
        private string _sadrzaj;
        private Korisnik k;

        public int _idDogadjaja;

        public KomunikacijaViewModel()
        {
            ZadaciMode = true;
        }

        public KomunikacijaViewModel(bool zadaciMode)
        {
            ZadaciMode = zadaciMode;
        }

        public ObservableCollection<Komentar> Komentari
        {
            get { return _komentari; }
            set
            {
                _komentari = value;
                OnPropertyChanged(nameof(Komentari));
            }
        }

        private bool _zadaciMode;
        public bool ZadaciMode
        {
            get => _zadaciMode;
            set
            {
                _zadaciMode = value;
                OnPropertyChanged(nameof(ZadaciMode));
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
            if (string.IsNullOrWhiteSpace(Sadrzaj))
            {
                return;
            }

            KorisnikStore korisnikStore = KorisnikStore.Instance;
            k = korisnikStore.TrenutniKorisnik;
            if (ZadaciMode)
            {
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
            else
            {
                // komunikacija klijenta i organizatora
                using (var db = new DatabaseContext())
                {
                    var dogadjaj = db.Dogadjaji.Include("Komentari")
                                               .SingleOrDefault(d => d.Id == _idDogadjaja);
                    var autor = db.Korisnici.SingleOrDefault(s => s.Email == k.Email);
                    Komentar komentar = new Komentar();
                    komentar.DatumVremeKomentara = DateTime.Now;
                    komentar.Sadrzaj = Sadrzaj;
                    komentar.Autor = autor;
                    dogadjaj.Komentari.Add(komentar);
                    db.Komentari.Add(komentar);
                    db.SaveChanges();
                }
                Sadrzaj = "";
                Komentari = KomentarService.getKomentareZaDogadjaj(_idDogadjaja);
            }
        }
        public void loadKomentari()
        {
            Komentari = KomentarService.getKomentareZaZadatak(IdZadatka);

        }

        private ICommand _zatvoriCommand;
        public ICommand ZatvoriCommand
        {
            get
            {
                if (_zatvoriCommand == null)
                    _zatvoriCommand = new RelayCommand(window => Zatvori((Window) window));
                return _zatvoriCommand;
            }
        }

        private void Zatvori(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
