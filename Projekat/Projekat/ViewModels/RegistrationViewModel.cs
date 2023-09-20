using Geocoding;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ICommand _submitCommand;
        private ICommand _cancelCommand;
        private readonly IKorisnikService KorisnikService = new KorisnikService(); 


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
        private string _ponovljenalozinka;
        public string PonovljenaLozinka
        {
            get => _ponovljenalozinka;
            set
            {
                _ponovljenalozinka = value;
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
                    _submitCommand = new RelayCommand(window => Register((Window) window));
                return _submitCommand;
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

        private void Cancel(Window window)
        {
            Dialog dialog = new Dialog();
            DialogViewModel viewModel = new DialogViewModel();
            viewModel._message = "Da li želite da odustanete?";
            dialog.DataContext = viewModel;
            dialog.Owner = window;
            dialog.ShowDialog();

            if (viewModel.odgovor == "Da")
            {
                _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
            }


        }
        private void Register(Window window)
        {
            string validationMessage = ValidationMessage();
            
            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
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
                    Klijent klijent = new Klijent();
                    klijent.Ime = Ime;
                    klijent.Prezime = Prezime;
                    klijent.Email = Email;
                    klijent.BrojTelefona = BrojTelefona;
                    klijent.Lozinka = Lozinka;

                    Adresa adresa = new Adresa();
                    adresa.Ulica = Ulica;
                    adresa.Broj = int.Parse(Broj);
                    adresa.Drzava = Drzava;
                    adresa.Grad = Grad;
                    klijent.Adresa = adresa;
                    db.Klijenti.Add(klijent);
                    db.SaveChanges();
                }

                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste se registrovali!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

                _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
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
            if (string.IsNullOrWhiteSpace(Email))
            {
                message += "Morate navesti email adresu!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Lozinka))
            {
                message += "Morate navesti lozinku!\n\n";
            }
            if (string.IsNullOrWhiteSpace(PonovljenaLozinka))
            {
                message += "Morate navesti ponovljenu lozinku!\n\n";
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
            if(string.IsNullOrWhiteSpace(BrojTelefona))
            {
                message += "Morate navesti broj telefona!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Broj))
            {
                message += "Morate navesti broj!\n\n";
            }
            if (KorisnikService.checkIfExist(Email))
            {
                message += "Navedena email adresa već postoji!\n\n";
            }
            try
            {
                var MailAddress = new MailAddress(Email).Address;
            }
            catch (FormatException)
            {
                message += "Email adresa nije u validnom obliku!\n\n";
            }
            if (!PonovljenaLozinka.Equals(Lozinka))
            {
                message += "Lozinke se moraju poklapati!\n\n";
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
        public RegistrationViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

        }
    }
}
