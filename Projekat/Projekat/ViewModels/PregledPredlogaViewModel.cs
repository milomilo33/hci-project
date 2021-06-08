using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
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
    public class PregledPredlogaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public PregledPredlogaViewModel(NavigationStore navigationStore, Predlog predlog, List<Zadatak> zadaci)
        {
            _navigationStore = navigationStore;

            Predlog = predlog;
            Zadaci = new ObservableCollection<Zadatak>(zadaci);

            UkupnaCena = predlog.Ponuda.Cena;
            // racunanje ukupne cene
            foreach (Zadatak zad in zadaci)
            {
                if (zad.Tip == Zadatak.TipZadatka.DODATNI)
                {
                    UkupnaCena += zad.IzabraniPredlog.Ponuda.Cena;
                }
            }

            AdresaString = Predlog.Ponuda.Saradnik.Adresa.ToString();
        }

        private Predlog _predlog;
        public Predlog Predlog
        {
            get { return _predlog; }
            set
            {
                _predlog = value;
                OnPropertyChanged(nameof(Predlog));
            }
        }

        private ObservableCollection<Zadatak> _zadaci;
        public ObservableCollection<Zadatak> Zadaci
        {
            get { return _zadaci; }
            set
            {
                _zadaci = value;
                OnPropertyChanged(nameof(Zadaci));
            }
        }

        private double _ukupnaCena;
        public double UkupnaCena
        {
            get { return _ukupnaCena; }
            set
            {
                _ukupnaCena = value;
                OnPropertyChanged(nameof(UkupnaCena));
            }
        }

        private string _adresaString;
        public string AdresaString
        {
            get { return _adresaString; }
            set
            {
                _adresaString = value;
                OnPropertyChanged(nameof(AdresaString));
            }
        }

        private ICommand _otvoriDodatneZahteveCommand;
        public ICommand OtvoriDodatneZahteveCommand
        {
            get
            {
                if (_otvoriDodatneZahteveCommand == null)
                    _otvoriDodatneZahteveCommand = new RelayCommand(_otvoriDodatneZahteveCommand => OtvoriDodatneZahteve());
                return _otvoriDodatneZahteveCommand;
            }
        }

        private void OtvoriDodatneZahteve()
        {
            // prozor dodatnih zahteva
        }

        private ICommand _otvoriRasporedSedenjaCommand;
        public ICommand OtvoriRasporedSedenjaCommand
        {
            get
            {
                if (_otvoriRasporedSedenjaCommand == null)
                    _otvoriRasporedSedenjaCommand = new RelayCommand(_otvoriRasporedSedenjaCommand => OtvoriRasporedSedenja());
                return _otvoriRasporedSedenjaCommand;
            }
        }

        private void OtvoriRasporedSedenja()
        {
            Console.WriteLine("raspored sedenja!");

            using (var db = new DatabaseContext())
            {
                int idDogadjaja = Zadaci.First().Dogadjaj.Id;
                Dogadjaj dogadjaj = db.Dogadjaji.Include("NerasporedjeniGosti")
                                                .Include("RasporedSedenja")
                                                .Include("RasporedSedenja.GostiZaStolom")
                                                .Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik.Stolovi.GostiZaStolom")
                                                .SingleOrDefault(d => d.Id == idDogadjaja);

                //if (dogadjaj.NerasporedjeniGosti == null)
                //{
                //    dogadjaj.NerasporedjeniGosti = new List<Gost>();
                //}
                if (dogadjaj.RasporedSedenja == null || dogadjaj.RasporedSedenja.Count() == 0)
                {
                    List<KapacitetStola> rasporedSedenja = new List<KapacitetStola>();
                    foreach (Zadatak zad in dogadjaj.Zadaci)
                    {
                        if (zad.Tip == Zadatak.TipZadatka.GLAVNI)
                        {
                            List<KapacitetStola> kapacitetiStolovaUPonudi = zad.IzabraniPredlog.Ponuda.Saradnik.Stolovi;
                            foreach (KapacitetStola ks in kapacitetiStolovaUPonudi)
                            {
                                KapacitetStola kapacitetSaGostima = new KapacitetStola();
                                kapacitetSaGostima.Kapacitet = ks.Kapacitet;
                                kapacitetSaGostima.Naziv = ks.Naziv;
                                kapacitetSaGostima.GostiZaStolom = new List<Gost>();
                                rasporedSedenja.Add(kapacitetSaGostima);
                            }

                            break;
                        }
                    }

                    dogadjaj.RasporedSedenja = rasporedSedenja;
                }

                db.SaveChanges();
                _navigationStore.CurrentViewModel = new RasporedSedenjaViewModel(_navigationStore, _navigationStore.CurrentViewModel, dogadjaj);
            }
        }

        private ICommand _prihvatiPredlogCommand;
        public ICommand PrihvatiPredlogCommand
        {
            get
            {
                if (_prihvatiPredlogCommand == null)
                    _prihvatiPredlogCommand = new RelayCommand(_prihvatiPredlogCommand => PrihvatiPredlog());
                return _prihvatiPredlogCommand;
            }
        }

        private void PrihvatiPredlog()
        {
            Console.WriteLine("prihvacen predlog!");
        }

        private ICommand _zatraziIzmeneCommand;
        public ICommand ZatraziIzmeneCommand
        {
            get
            {
                if (_zatraziIzmeneCommand == null)
                    _zatraziIzmeneCommand = new RelayCommand(_zatraziIzmeneCommand => ZatraziIzmene());
                return _zatraziIzmeneCommand;
            }
        }

        private void ZatraziIzmene()
        {
            Console.WriteLine("zatrazi izmene!");
        }

        private ICommand _povratakCommand;

        public ICommand PovratakCommand
        {
            get
            {
                if (_povratakCommand == null)
                    _povratakCommand = new RelayCommand(_povratakCommand => Povratak());
                return _povratakCommand;
            }
        }

        public void Povratak()
        {
            _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore, true);
        }
    }
}
