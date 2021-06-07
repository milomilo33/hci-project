using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AddSaradnikViewModel : ViewModelBase
    {
        public AddSaradnikViewModel()
        {
        }

        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }

        private string _opis;
        public string Opis
        {
            get => _opis;
            set
            {
                _opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }

        private string _tip;
        public string Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                OnPropertyChanged(nameof(Tip));
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

        private ICommand _addSardadnikCommand;
        public ICommand AddSaradnikCommand
        {
            get
            {
                if (_addSardadnikCommand == null)
                    _addSardadnikCommand = new RelayCommand(_addSardadnikCommand => AddEvent());
                return _addSardadnikCommand;
            }
        }

        private void AddEvent()
        {
            using (var db = new DatabaseContext())
            {
                if (string.IsNullOrWhiteSpace(Grad) ||
                    string.IsNullOrWhiteSpace(Ulica) ||
                    string.IsNullOrWhiteSpace(Broj) ||
                    string.IsNullOrWhiteSpace(Naziv) ||
                    string.IsNullOrWhiteSpace(Tip) ||
                    string.IsNullOrWhiteSpace(Drzava) ||
                    string.IsNullOrWhiteSpace(BrojTelefona))
                {
                    MessageBox.Show("Morate uneti podatke u sva polja forme.", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Saradnik saradnik = new Saradnik();
                Adresa adresa = new Adresa();

                adresa.Grad = Grad;
                adresa.Ulica = Ulica;
                int.TryParse(Broj, out int result);
                if (result == 0)
                {
                    return;
                }
                adresa.Broj = result;
                adresa.Drzava = Drzava;

                saradnik.Naziv = Naziv;
                saradnik.Tip = Tip;
                saradnik.Adresa = adresa;
                saradnik.BrojTelefona = BrojTelefona;
                saradnik.Opis = Opis;

                db.Adrese.Add(adresa);
                db.Saradnici.Add(saradnik);

                db.SaveChanges();
                Console.WriteLine("AAAAAAAAADED");
            }
        }

        /*public async Task<string> GetLocationFromAddressAsync()
        {
            string address = $"{Drzava} {Grad} {Ulica} {Broj}";

            var httpClient = new HttpClient();
            var httpResult = await httpClient.GetAsync(
                "http://nominatim.openstreetmap.org/search?q=135+pilkington+avenue,+birmingham&format=json&polygon=1&addressdetails=1");

            var result = await httpResult.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            var r = (JArray)JsonConvert.DeserializeObject(result);
            var latString = ((JValue)r[0]["lat"]).Value as string;
            var longString = ((JValue)r[0]["lon"]).Value as string;

            return latString + " " + longString;

        }*/
    }
}
