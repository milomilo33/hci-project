using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AddOrganizatorViewModel : ViewModelBase
    {

        public AddOrganizatorViewModel()
        {
        }

        private bool _emailExists = false;
        public bool EmailExists
        {
            get => _emailExists;
            set
            {
                _emailExists = value;
                OnPropertyChanged(nameof(EmailExists));
            }
        }

        private bool _lozinkaCheck = false;
        public bool LozinkaCheck
        {
            get => _lozinkaCheck;
            set
            {
                _lozinkaCheck = value;
                OnPropertyChanged(nameof(LozinkaCheck));
            }
        }

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

        private string _ponovljenaLozinka;
        public string PonovljenaLozinka
        {
            get => _ponovljenaLozinka;
            set
            {
                _ponovljenaLozinka = value;
                OnPropertyChanged(nameof(PonovljenaLozinka));
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

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(_addCommand => AddEvent());
                return _addCommand;
            }
        }

        private void AddEvent()
        {
            using (var db = new DatabaseContext())
            {
                if (string.IsNullOrWhiteSpace(Grad) ||
                    string.IsNullOrWhiteSpace(Ulica) ||
                    string.IsNullOrWhiteSpace(Broj) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Ime) ||
                    string.IsNullOrWhiteSpace(Prezime) ||
                    string.IsNullOrWhiteSpace(BrojTelefona))
                {
                    MessageBox.Show("Morate uneti podatke u sva polja forme.", "Upozorenje!", MessageBoxButton.OK ,MessageBoxImage.Warning);
                    return;
                }

                Organizator organizator = new Organizator();
                Adresa adresa = new Adresa();

                adresa.Grad = Grad;
                adresa.Ulica = Ulica;
                int.TryParse(Broj, out int result);
                if(result == 0)
                {
                    return;
                }
                adresa.Broj = result;
                adresa.Drzava = Drzava;

                if(db.Organizatori.Any(o => o.Email == Email)) {
                    EmailExists = true;
                    return;
                }

                organizator.Email = Email;
                organizator.Ime = Ime;
                organizator.Prezime = Prezime;
                organizator.Adresa = adresa;
                organizator.BrojTelefona = BrojTelefona;

                if(Lozinka != PonovljenaLozinka)
                {
                    LozinkaCheck = true;
                    return;
                }
                organizator.Lozinka = Lozinka;

                db.Adrese.Add(adresa);
                db.Organizatori.Add(organizator);
                
                db.SaveChanges();
                EmailExists = false;
                LozinkaCheck = false;
                Console.WriteLine("AAAAAAAAADED");
            }
        }
    }
}