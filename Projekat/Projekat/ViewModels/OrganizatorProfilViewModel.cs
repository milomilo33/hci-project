using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class OrganizatorProfilViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewModelBase _previousViewModel;

        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _povratakCommand;
        public Organizator organizator;


        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _ime;
        public string Ime
        {
            get => _ime;
            set
            {
                _ime = value;
                OnPropertyChanged(nameof(Ime));
            }
        }
        private string _prezime;
        public string Prezime
        {
            get => _prezime;
            set
            {
                _prezime = value;
                OnPropertyChanged(nameof(Prezime));
            }
        }
        private string _brojTelefona;
        public string BrojTelefona
        {
            get => _brojTelefona;
            set
            {
                _brojTelefona = value;
                OnPropertyChanged(nameof(BrojTelefona));
            }
        }
        private string _ulica;
        public string Ulica
        {
            get => _ulica;
            set
            {
                _ulica = value;
                OnPropertyChanged(nameof(Ulica));
            }
        }
        private int _broj;
        public int Broj
        {
            get => _broj;
            set
            {
                _broj = value;
                OnPropertyChanged(nameof(Broj));
            }
        }
        private string _grad;
        public string Grad
        {
            get => _grad;
            set
            {
                _grad = value;
                OnPropertyChanged(nameof(Grad));
            }
        }
        private string _drzava;
        public string Drzava
        {
            get => _drzava;
            set
            {
                _drzava = value;
                OnPropertyChanged(nameof(Drzava));
            }
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(window => Edit((Window) window));
                return _editCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(window => Cancel((Window) window));
                return _cancelCommand;
            }
        }

        private void Edit(Window window)
        {
            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
            if (organizator.Ime == Ime && organizator.Prezime == Prezime && organizator.BrojTelefona == BrojTelefona
                && organizator.Adresa.Ulica == Ulica && organizator.Adresa.Broj == Broj &&
                organizator.Adresa.Drzava == Drzava && organizator.Adresa.Grad == Grad)
            {
                dialogModel.IsError = true;
                dialogModel.Message = "Ništa nije izmenjeno!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else
            {

                using (var db = new DatabaseContext())
                {
                    organizator = db.Organizatori.Include("Adresa").SingleOrDefault(o => o.Email == Email);
                    organizator.Ime = Ime;
                    organizator.Prezime = Prezime;
                    organizator.BrojTelefona = BrojTelefona;
                    organizator.Adresa.Ulica = Ulica;
                    organizator.Adresa.Grad = Grad;
                    organizator.Adresa.Broj = Broj;
                    organizator.Adresa.Drzava = Drzava;
                    db.SaveChanges();
                }

                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste izmenili svoje podatke!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
                Povratak();
            }

           
        }
        private void Cancel(Window window)
        {
            Dialog dialog = new Dialog();
            DialogViewModel viewModel = new DialogViewModel();
            viewModel._message = "Da li želite da odustanete?";
            dialog.DataContext = viewModel;
            dialog.Owner = window;
            dialog.ShowDialog();

            if(viewModel.odgovor == "Da")
            {
                Povratak();
            }

        }
        private Organizator LoadOrganizator(String email)
        {
            Organizator organizator;
            using (var db = new DatabaseContext())
            {
                organizator = db.Organizatori.Include("Adresa").SingleOrDefault(o =>o.Email == email);
            }
            return organizator;

        }

        public OrganizatorProfilViewModel(NavigationStore navigationStore, ViewModelBase viewModelBase)
        {
            _navigationStore = navigationStore;
            _previousViewModel = viewModelBase;
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            organizator = LoadOrganizator(korisnikStore.TrenutniKorisnik.Email);
            Email = organizator.Email;
            Ime = organizator.Ime;
            Prezime = organizator.Prezime;
            BrojTelefona = organizator.BrojTelefona;
            Ulica = organizator.Adresa.Ulica;
            Grad = organizator.Adresa.Grad;
            Drzava = organizator.Adresa.Drzava;
            Broj = organizator.Adresa.Broj;

            
            

        }

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
