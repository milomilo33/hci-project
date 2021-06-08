using Geocoding;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AddOrganizatorViewModel : ViewModelBase
    {

        public AddOrganizatorViewModel()
        {
        }

        
        public bool EmailValid { get; set; }

        private bool _isEdit;
        public bool IsEdit 
        { 
            get => _isEdit; 
            set
            {
                _isEdit = value;
                OnPropertyChanged(nameof(IsEdit));
            }
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

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(_editCommand => EditEvent());
                return _editCommand;
            }
        }

        private void EditEvent()
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
                    MessageBox.Show("Morate uneti podatke u sva polja forme.", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }



                Organizator organizator = db.Organizatori.Include("Adresa").FirstOrDefault(o => o.Email == Email); 
                Adresa adresa = organizator.Adresa;
                Console.WriteLine(Grad);
                adresa.Grad = Grad;
                adresa.Ulica = Ulica;
                int.TryParse(Broj, out int result);
                if (result == 0)
                {
                    return;
                }
                adresa.Broj = result;
                adresa.Drzava = Drzava;

                organizator.Email = Email;
                organizator.Ime = Ime;
                organizator.Prezime = Prezime;
                organizator.Adresa = adresa;
                organizator.BrojTelefona = BrojTelefona;

                if (Lozinka != PonovljenaLozinka)
                {
                    LozinkaCheck = true;
                    return;
                }
                organizator.Lozinka = Lozinka;


                db.SaveChanges();
                EmailExists = false;
                LozinkaCheck = false;
                Console.WriteLine("AAAAAAAAADED");
            }
        }

        public (double, double) GetLocationFromAddressAsync()
        {
            double lat = -1, lon = -1;

            string address = $"{Drzava} {Grad} {Ulica} {Broj}";
            JArray result;

            string query = $"q={address}&polygon_geojson=1&format=jsonv2&limit=5";
            var req = (HttpWebRequest)HttpWebRequest.Create("https://nominatim.openstreetmap.org/search.php?" + query);
            req.Method = "GET";
            req.UserAgent = ".NET Framework Test Client";
            using (var resp = req.GetResponse())
            {
                var res = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                result = JArray.Parse(res);

            }

            if (result.IsNullOrEmpty())
            {
                return (-1, -1);
            }

            var properties = result.Children<JObject>().Properties();

            foreach (JProperty property in properties)
            {
                if (property.Name == "lat")
                {
                    lat = double.Parse(property.Value.ToString());
                }
                if (property.Name == "lon")
                {
                    lon = double.Parse(property.Value.ToString());
                }
            }
            return (lat, lon);

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

                try
                {
                    var address = new MailAddress(Email).Address;
                }
                catch (FormatException)
                {
                    EmailValid = true;
                    OnPropertyChanged(nameof(EmailValid));
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

                if (GetLocationFromAddressAsync() == (-1, -1))
                {
                    System.Windows.MessageBox.Show("Adresa koju ste uneli ne postoji", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                EmailValid = false;
                OnPropertyChanged(nameof(EmailValid));
                Console.WriteLine("ADDED Organizator");
            }
        }
    }
}