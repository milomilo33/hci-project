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
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ICommand _submitCommand;
        private ICommand _cancelCommand;


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
        private string _broj;
        public string Broj
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
        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                    _submitCommand = new RelayCommand(_submitCommand => Register());
                return _submitCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(_cancelCommand => Cancel());
                return _cancelCommand;
            }
        }

        private void Cancel()
        {
           
        }
        private void Register()
        {
            using (var db = new DatabaseContext())
            {
                Klijent klijent = new Klijent();
                klijent.Ime = Ime;
                klijent.Prezime = Prezime;
                klijent.Email = Email;
                klijent.BrojTelefona = BrojTelefona;
                klijent.Lozinka = "123";


                Adresa a = new Adresa();
                a.Broj = int.Parse(Broj);
                a.Ulica = Ulica;
                a.Grad = Grad;
                a.Drzava = Drzava;
                db.Adrese.Add(a);
                klijent.Adresa = a;
                db.Klijenti.Add(klijent);
                db.SaveChanges();
            }
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);

        }
        public RegistrationViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

        }
    }
}
