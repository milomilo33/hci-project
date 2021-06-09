using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
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
    public class KlijentKreiranjeDogadjajaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private readonly ViewModelBase _previousViewModel;

        public KlijentKreiranjeDogadjajaViewModel(NavigationStore navigationStore, ViewModelBase viewModelBase)
        {
            _navigationStore = navigationStore;
            _previousViewModel = viewModelBase;

            using (var db = new DatabaseContext())
            {
                List<string> sveVrsteProslave = new List<string>();

                foreach (Dogadjaj d in db.Dogadjaji)
                {
                    sveVrsteProslave.Add(d.VrstaProslave);
                }
                sveVrsteProslave = sveVrsteProslave.Distinct().ToList();

                SveVrsteProslave = new ObservableCollection<string>(sveVrsteProslave);
            }

            BudzetPromenljiv = true;
        }

        private string _vrsta;
        public string Vrsta
        {
            get { return _vrsta; }
            set
            {
                _vrsta = value;
                OnPropertyChanged(nameof(Vrsta));
            }
        }

        private string _tema;
        public string Tema
        {
            get { return _tema; }
            set
            {
                _tema = value;
                OnPropertyChanged(nameof(Tema));
            }
        }

        private Organizator _organizator;
        public Organizator Organizator
        {
            get { return _organizator; }
            set
            {
                _organizator = value;
                OnPropertyChanged(nameof(Organizator));
            }
        }

        private string _imeOrganizatora;
        public string ImeOrganizatora
        {
            get { return _imeOrganizatora; }
            set
            {
                _imeOrganizatora = value;
                OnPropertyChanged(nameof(ImeOrganizatora));
            }
        }

        private int _budzet;
        public int Budzet
        {
            get { return _budzet; }
            set
            {
                _budzet = value;
                OnPropertyChanged(nameof(Budzet));
            }
        }

        private bool _budzetPromenljiv;
        public bool BudzetPromenljiv
        {
            get { return _budzetPromenljiv; }
            set
            {
                _budzetPromenljiv = value;
                OnPropertyChanged(nameof(BudzetPromenljiv));
            }
        }

        private DateTime? _datum;
        public DateTime? DatumOdrzavanja
        {
            get { return _datum; }
            set
            {
                _datum = value;
                OnPropertyChanged(nameof(DatumOdrzavanja));
            }
        }

        private int _broj;
        public int BrojGostiju
        {
            get { return _broj; }
            set
            {
                _broj = value;
                OnPropertyChanged(nameof(BrojGostiju));
            }
        }

        private string _dodatniZahtevi;
        public string DodatniZahtevi
        {
            get { return _dodatniZahtevi; }
            set
            {
                _dodatniZahtevi = value;
                OnPropertyChanged(nameof(DodatniZahtevi));
            }
        }

        private string _mesto;
        public string MestoOdrzavanja
        {
            get { return _mesto; }
            set
            {
                _mesto = value;
                OnPropertyChanged(nameof(MestoOdrzavanja));
            }
        }

        public ObservableCollection<string> SveVrsteProslave { get; set; }

        private bool _dodavanjeMode;
        public bool DodavanjeMode
        {
            get { return _dodavanjeMode; }
            set
            {
                _dodavanjeMode = value;
                OnPropertyChanged(nameof(DodavanjeMode));
            }
        }

        private ICommand _odaberiOrganizatoraCommand;

        public ICommand OdaberiOrganizatoraCommand
        {
            get
            {
                if (_odaberiOrganizatoraCommand == null)
                    _odaberiOrganizatoraCommand = new RelayCommand(_odaberiOrganizatoraCommand => OdaberiOrganizatora());
                return _odaberiOrganizatoraCommand;
            }
        }

        public void OdaberiOrganizatora()
        {
            _navigationStore.CurrentViewModel = new KlijentKreiranjeDogadjajaOdabirOrganizatoraViewModel(_navigationStore, _navigationStore.CurrentViewModel);
        }

        private ICommand _obrisiOrganizatoraCommand;

        public ICommand ObrisiOrganizatoraCommand
        {
            get
            {
                if (_obrisiOrganizatoraCommand == null)
                    _obrisiOrganizatoraCommand = new RelayCommand(_obrisiOrganizatoraCommand => ObrisiOrganizatora());
                return _obrisiOrganizatoraCommand;
            }
        }

        public void ObrisiOrganizatora()
        {
            Organizator = null;
            ImeOrganizatora = null;
        }

        private ICommand _dodajVrstuCommand;

        public ICommand DodajVrstuCommand
        {
            get
            {
                if (_dodajVrstuCommand == null)
                    _dodajVrstuCommand = new RelayCommand(_dodajVrstuCommand => DodajVrstuToggle());
                return _dodajVrstuCommand;
            }
        }

        private void DodajVrstuToggle()
        {
            DodavanjeMode = !DodavanjeMode;
        }

        private ICommand _kreirajDogadjajCommand;

        public ICommand KreirajDogadjajCommand
        {
            get
            {
                if (_kreirajDogadjajCommand == null)
                    _kreirajDogadjajCommand = new RelayCommand(window => KreirajDogadjaj((Window) window));
                return _kreirajDogadjajCommand;
            }
        }

        private void KreirajDogadjaj(Window window)
        {
            string validationMessage = ValidationMessage();

            if (string.IsNullOrWhiteSpace(validationMessage))
            {
                // kreiraj dogadjaj

                Dogadjaj noviDogadjaj = new Dogadjaj();
                noviDogadjaj.BrojGostiju = BrojGostiju;
                noviDogadjaj.Budzet = Budzet;
                noviDogadjaj.BudzetPromenljiv = BudzetPromenljiv;
                noviDogadjaj.DatumOdrzavanja = (DateTime) DatumOdrzavanja;
                noviDogadjaj.DodatniZahtevi = DodatniZahtevi;
                noviDogadjaj.MestoOdrzavanja = MestoOdrzavanja;
                //noviDogadjaj.Organizator = Organizator;
                noviDogadjaj.StatusEnum = Organizator == null ? Dogadjaj.STATUS_DOGADJAJA.NEDODELJEN : Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR;
                noviDogadjaj.Tema = Tema;
                noviDogadjaj.VrstaProslave = Vrsta;

                Klijent trenutniKlijent = (Klijent) KorisnikStore.Instance.TrenutniKorisnik;

                using (var db = new DatabaseContext())
                {
                    db.Dogadjaji.Add(noviDogadjaj);
                    Klijent dbKlijent = db.Klijenti.SingleOrDefault(k => k.Email.Equals(trenutniKlijent.Email));
                    //db.Entry(dbKlijent).State = System.Data.Entity.EntityState.Detached;
                    if (dbKlijent.Dogadjaji == null)
                    {
                        dbKlijent.Dogadjaji = new List<Dogadjaj>();
                    }
                    if (Organizator != null)
                    {
                        noviDogadjaj.Organizator = db.Organizatori.SingleOrDefault(o => o.Email.Equals(Organizator.Email));
                    }
                    dbKlijent.Dogadjaji.Add(noviDogadjaj);
                    db.SaveChanges();
                }

                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste kreirali događaj!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

                Povratak();
            }
            else
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = validationMessage;
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
        }

        private string ValidationMessage()
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(Vrsta))
            {
                message += "Morate odabrati vrstu proslave!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Tema))
            {
                message += "Morate uneti temu proslave!\n\n";
            }
            if (Budzet <= 0)
            {
                message += "Morate navesti budžet za događaj!\n\n";
            }
            if (DatumOdrzavanja == null)
            {
                message += "Morate odabrati datum proslave!\n\n";
            }
            if (string.IsNullOrWhiteSpace(MestoOdrzavanja))
            {
                message += "Morate navesti mesto održavanja događaja!\n\n";
            }

            return message;
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
            _navigationStore.CurrentViewModel = _previousViewModel;
        }

        private ICommand _pocetnaStranicaCommand;

        public ICommand PocetnaStranicaCommand
        {
            get
            {
                if (_pocetnaStranicaCommand == null)
                    _pocetnaStranicaCommand = new RelayCommand(_pocetnaStranicaCommand => PocetnaStrana());
                return _pocetnaStranicaCommand;
            }
        }

        private void PocetnaStrana()
        {
            KorisnikStore korisnik = KorisnikStore.Instance;
            Korisnik k = korisnik.TrenutniKorisnik;

            if (k.GetType() == typeof(Administrator))
            {
                _navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
            }
            else if (k.GetType() == typeof(Organizator))
            {
                _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
            }
            else
            {
                _navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
            }
        }
    }
}
