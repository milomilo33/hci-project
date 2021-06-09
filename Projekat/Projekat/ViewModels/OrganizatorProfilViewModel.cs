using Geocoding;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class OrganizatorProfilViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _povratakCommand;
        private ICommand _promenaLozinkeCommand;
        public Korisnik korisnik;


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
            string validationMessage = ValidationMessage();

            
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {

                dialogModel.IsError = true;
                dialogModel.Message = validationMessage;
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else
            {

                using (var db = new DatabaseContext())
                {
                    korisnik = db.Korisnici.Include("Adresa").SingleOrDefault(o => o.Email == Email);
                    korisnik.Ime = Ime;
                    korisnik.Prezime = Prezime;
                    korisnik.BrojTelefona = BrojTelefona;
                    korisnik.Adresa.Ulica = Ulica;
                    korisnik.Adresa.Grad = Grad;
                    korisnik.Adresa.Broj = int.Parse(Broj);
                    korisnik.Adresa.Drzava = Drzava;
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
        private string ValidationMessage()
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(Ime))
            {
                message += "Morate navesti ime!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Prezime))
            {
                message += "Morate navesti prezime!\n\n";
            }
           
            if (string.IsNullOrWhiteSpace(Ulica))
            {
                message += "Morate navesti ulicu!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Grad))
            {
                message += "Morate navesti grad!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Drzava))
            {
                message += "Morate navesti državu!\n\n";
            }
            if (string.IsNullOrWhiteSpace(BrojTelefona))
            {
                message += "Morate navesti broj telefona!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Broj))
            {
                message += "Morate navesti broj!\n\n";
            }
            
           
            if (GetLocationFromAddressAsync() == (-1, -1))
            {
                Console.WriteLine(GetLocationFromAddressAsync());
                message += "Navedena  adresa ne postoji!\n\n";
            }


            return message;
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
        private Korisnik LoadOrganizator(String email)
        {
            Korisnik korisnik;
            using (var db = new DatabaseContext())
            {
                korisnik = db.Korisnici.Include("Adresa").SingleOrDefault(o =>o.Email == email);
            }
            return korisnik;

        }

        public OrganizatorProfilViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            korisnik = LoadOrganizator(korisnikStore.TrenutniKorisnik.Email);
            Email = korisnik.Email;
            Ime = korisnik.Ime;
            Prezime = korisnik.Prezime;
            BrojTelefona = korisnik.BrojTelefona;
            Ulica = korisnik.Adresa.Ulica;
            Grad = korisnik.Adresa.Grad;
            Drzava = korisnik.Adresa.Drzava;
            Broj = korisnik.Adresa.Broj.ToString();

            
            

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

        public ICommand PromenaLozinkeCommand
        {
            get
            {
                if (_promenaLozinkeCommand == null)
                    _promenaLozinkeCommand = new RelayCommand(_promenaLozinkeCommand => PromenaLozinke());
                return _promenaLozinkeCommand;
            }
        }

        public void PromenaLozinke()
        {
            _navigationStore.CurrentViewModel = new PromenaLozinkeViewModel(_navigationStore);
        }
        public void Povratak()
        {

            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
        }

    }
}
