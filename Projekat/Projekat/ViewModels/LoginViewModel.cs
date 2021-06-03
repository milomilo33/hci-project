using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

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

        private string _lozinka;
        public string Lozinka
        {
            get => _lozinka;
            set
            {
                _lozinka = value;
                OnPropertyChanged(nameof(Lozinka));
            }
        }

        private ICommand _loginCommand;
        private ICommand _registrationCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(_loginCommand => LoginUser());
                return _loginCommand;
            }
        }

        private void LoginUser()
        {
            using (var db = new DatabaseContext())
            {
                foreach (Korisnik k in db.Korisnici)
                {
                    if (k.Email.Equals(Email))
                    {
                        if (!k.Lozinka.Equals(Lozinka))
                        {
                            Console.WriteLine("Nevalidna lozinka");
                            return;
                        }

                        KorisnikStore korisnikStore = KorisnikStore.Instance;
                        korisnikStore.TrenutniKorisnik = k;

                        if (k.GetType() == typeof(Administrator))
                        {
                            Console.WriteLine("Administrator");
                            return;
                        }
                        else if (k.GetType() == typeof(Klijent))
                        {
                            Console.WriteLine("Klijent");
                            return;
                        }
                        else if (k.GetType() == typeof(Organizator))
                        {
                            Console.WriteLine("Organizator");
                            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Login error");
                            return;
                        }
                    }
                }

                Console.WriteLine("Nepostojeci korisnik");
            }
        }

        public ICommand RegistrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                    _registrationCommand = new RelayCommand(_registrationCommand => Registration());
                return _registrationCommand;
            }
        }
        private void Registration()
        {

            _navigationStore.CurrentViewModel = new RegistrationViewModel();
        }


        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}


