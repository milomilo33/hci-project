using CsvHelper;
using CsvHelper.Configuration;
using Geocoding;
using Geocoding.Microsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AddSaradnikViewModel : ViewModelBase
    {
        public AddSaradnikViewModel()
        {
            Stolovi = new List<KapacitetStola>();
        }

        public bool IsEdit { get; set; }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private List<KapacitetStola> _stolovi;
        public List<KapacitetStola> Stolovi
        {
            get => _stolovi;
            set
            {
                _stolovi = value;
                OnPropertyChanged(nameof(Stolovi));
            }
        }

        private bool _isLokal;
        public bool IsLokal
        {
            get => _isLokal;
            set
            {
                _isLokal = value;
                OnPropertyChanged(nameof(IsLokal));
            }
        }

        private bool _isOther;
        public bool IsOther
        {
            get => _isOther;
            set
            {
                _isOther = value;
                OnPropertyChanged(nameof(IsOther));
            }
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

        private string _brojMesta;
        public string BrojMesta
        {
            get => _brojMesta;
            set
            {
                _brojMesta = value;
                OnPropertyChanged(nameof(BrojMesta));
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

        private ICommand _editSardadnikCommand;
        public ICommand EditSaradnikCommand
        {
            get
            {
                if (_editSardadnikCommand == null)
                    _editSardadnikCommand = new RelayCommand(_editSardadnikCommand => EditEvent());
                return _editSardadnikCommand;
            }
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand
        {
            get
            {
                if (_fileOpenCommand == null)
                    _fileOpenCommand = new RelayCommand(_fileOpenCommand => OpenFile());
                return _fileOpenCommand;
            }
        }

        private void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog fileManager = new Microsoft.Win32.OpenFileDialog();
            fileManager.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> result = fileManager.ShowDialog();
            if (result == true) {
                string filename = fileManager.FileName;
                ParseCSV(filename);
            }
                
        }

        private void ParseCSV(string filename)
        {
            var reader = File.OpenText(filename);
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };

            var csvReader = new CsvReader(reader, config);
            int kapacitetLokala = 0;

            while(csvReader.Read())
            {
                csvReader.TryGetField<string>(0, out string entity);

                string naziv = entity.Split(',')[0];
                int.TryParse(entity.Split(',')[1], out int kapacitet);

                if(kapacitet == 0)
                {
                    System.Windows.MessageBox.Show($"Vrednost kapaciteta stola {naziv} je nepravilna", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Stolovi = new List<KapacitetStola>();
                    return;
                }

                KapacitetStola ks = new KapacitetStola { Naziv = naziv, Kapacitet = kapacitet };
                if(Stolovi.Any(s => s.Naziv == naziv))
                {
                    System.Windows.MessageBox.Show("Nazivi stolova nisu jedinstveni u CSV fajlu", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Stolovi = new List<KapacitetStola>();
                    return;
                    
                }
                Stolovi.Add(ks);
                kapacitetLokala += kapacitet;

            }

            BrojMesta = kapacitetLokala.ToString();
        }

        private void EditEvent()
        {
            using (var db = new DatabaseContext())
            {
                if (string.IsNullOrWhiteSpace(Grad) ||
                    string.IsNullOrWhiteSpace(Ulica) ||
                    string.IsNullOrWhiteSpace(Broj) ||
                    string.IsNullOrWhiteSpace(Naziv) ||
                    string.IsNullOrWhiteSpace(Drzava) ||
                    string.IsNullOrWhiteSpace(BrojTelefona))
                {
                    Console.WriteLine(Tip);
                    Console.WriteLine(BrojMesta);
                    if (string.IsNullOrWhiteSpace(Tip) && string.IsNullOrWhiteSpace(BrojMesta))
                    {

                        System.Windows.MessageBox.Show("Morate uneti podatke u sva polja forme.", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                Saradnik saradnik = db.Saradnici.Include("Adresa").FirstOrDefault(o => o.Id == Id);

                Adresa adresa = saradnik.Adresa;

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
                saradnik.Adresa = adresa;
                saradnik.BrojTelefona = BrojTelefona;
                saradnik.Opis = Opis;

                if (IsLokal)
                {
                    Console.WriteLine(BrojMesta);
                    saradnik.BrojMesta = int.Parse(BrojMesta);
                    saradnik.Tip = "Lokal";
                }
                saradnik.Tip = Tip;

                
                db.SaveChanges();
                Console.WriteLine("edited");
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
                    string.IsNullOrWhiteSpace(Drzava) ||
                    string.IsNullOrWhiteSpace(BrojTelefona))
                {
                    Console.WriteLine(Tip);
                    Console.WriteLine(BrojMesta);
                    if (string.IsNullOrWhiteSpace(Tip) && string.IsNullOrWhiteSpace(BrojMesta))
                    {

                        System.Windows.MessageBox.Show("Morate uneti podatke u sva polja forme.", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                Saradnik saradnik = new Saradnik();
                Adresa adresa = new Adresa();

                if(GetLocationFromAddressAsync() == (-1, -1))
                {
                    System.Windows.MessageBox.Show("Adresa koju ste uneli ne postoji", "Upozorenje!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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
                saradnik.Adresa = adresa;
                saradnik.BrojTelefona = BrojTelefona;
                saradnik.Opis = Opis;

                Console.WriteLine(IsLokal);
                if (IsLokal == true)
                {
                    saradnik.BrojMesta = int.Parse(BrojMesta);
                    saradnik.Stolovi = Stolovi;
                    saradnik.Tip = "Lokal";
                } else
                {
                    saradnik.Tip = Tip;
                }

                db.Adrese.Add(adresa);
                db.Saradnici.Add(saradnik);

                db.SaveChanges();
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

            if(result.IsNullOrEmpty())
            {
                return (-1, -1);
            }

            var properties = result.Children<JObject>().Properties();

            foreach (JProperty property in properties)
            {
                if(property.Name == "lat")
                {
                    lat = double.Parse(property.Value.ToString());
                }
                if(property.Name == "lon")
                {
                    lon = double.Parse(property.Value.ToString());
                }
            }

            

            return (lat, lon);

        }
    }
}
