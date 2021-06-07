using Projekat.Commands;
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
