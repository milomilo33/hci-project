using Geocoding;
using Newtonsoft.Json.Linq;
using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
using Projekat.Utility;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class PregledPredlogaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public PregledPredlogaViewModel(NavigationStore navigationStore, Predlog predlog, List<Zadatak> zadaci, bool organizovan)
        {
            _navigationStore = navigationStore;
         
            Predlog = predlog;
            Zadaci = new ObservableCollection<Zadatak>(zadaci);

            UkupnaCena = predlog.Ponuda.Cena;
            // racunanje ukupne cene
            foreach (Zadatak zad in zadaci)
            {
                if (zad.Tip == Zadatak.TipZadatka.DODATNI)
                {
                    UkupnaCena += zad.IzabraniPredlog.Ponuda.Cena;
                }
            }

            AdresaString = Predlog.Ponuda.Saradnik.Adresa.ToString();
            _organizovan = organizovan;
            IsKlijent = KorisnikStore.Instance.TrenutniKorisnik is Klijent ? true : false;
        }

        private bool _isKlijent;
        public bool IsKlijent
        {
            get { return _isKlijent; }
            set
            {
                _isKlijent = value;
                OnPropertyChanged(nameof(IsKlijent));
            }
        }

        private bool _organizovan;
        public bool Organizovan
        {
            get { return _organizovan; }
            set
            {
                _organizovan = value;
                OnPropertyChanged(nameof(Organizovan));
            }
        }

        private Predlog _predlog;
        public Predlog Predlog
        {
            get { return _predlog; }
            set
            {
                _predlog = value;
                OnPropertyChanged(nameof(Predlog));
            }
        }

        private ObservableCollection<Zadatak> _zadaci;
        public ObservableCollection<Zadatak> Zadaci
        {
            get { return _zadaci; }
            set
            {
                _zadaci = value;
                OnPropertyChanged(nameof(Zadaci));
            }
        }

        private double _ukupnaCena;
        public double UkupnaCena
        {
            get { return _ukupnaCena; }
            set
            {
                _ukupnaCena = value;
                OnPropertyChanged(nameof(UkupnaCena));
            }
        }

        private string _adresaString;
        public string AdresaString
        {
            get { return _adresaString; }
            set
            {
                _adresaString = value;
                OnPropertyChanged(nameof(AdresaString));
            }
        }

        private ICommand _otvoriDodatneZahteveCommand;
        public ICommand OtvoriDodatneZahteveCommand
        {
            get
            {
                if (_otvoriDodatneZahteveCommand == null)
                    _otvoriDodatneZahteveCommand = new RelayCommand(_otvoriDodatneZahteveCommand => OtvoriDodatneZahteve());
                return _otvoriDodatneZahteveCommand;
            }
        }

        private void OtvoriDodatneZahteve()
        {
            using (var db = new DatabaseContext())
            {
                ObservableCollection<Predlog> dodatniZahtevi = new ObservableCollection<Predlog>();
                foreach (Zadatak zad in Zadaci)
                {
                    if (zad.Tip == Zadatak.TipZadatka.DODATNI)
                    {
                        Predlog dodatniPredlog = db.Zadaci.Include("IzabraniPredlog.Ponuda.Saradnik")
                                                          .SingleOrDefault(z => z.Id == zad.Id)
                                                          .IzabraniPredlog;
                        dodatniZahtevi.Add(dodatniPredlog);
                    }
                }

                _navigationStore.CurrentViewModel = new PregledPredlogaDodatniZahteviViewModel(_navigationStore, _navigationStore.CurrentViewModel, dodatniZahtevi);
            }
        }

        private ICommand _otvoriRasporedSedenjaCommand;
        public ICommand OtvoriRasporedSedenjaCommand
        {
            get
            {
                if (_otvoriRasporedSedenjaCommand == null)
                    _otvoriRasporedSedenjaCommand = new RelayCommand(window => OtvoriRasporedSedenja((Window) window));
                return _otvoriRasporedSedenjaCommand;
            }
        }

        public ObservableCollection<Gost> tempNerasporedjeniGosti { get; set; }
        public ObservableCollection<KapacitetStola> tempRasporedSedenja { get; set; }
        public Dogadjaj tempDogadjaj { get; set; }

        private void OtvoriRasporedSedenja(Window window)
        {
            if (tempNerasporedjeniGosti == null && tempRasporedSedenja == null)
            {
                using (var db = new DatabaseContext())
                {
                    int idDogadjaja = Zadaci.First().Dogadjaj.Id;
                    Dogadjaj dogadjaj = db.Dogadjaji.Include("NerasporedjeniGosti")
                                                    .Include("RasporedSedenja")
                                                    .Include("RasporedSedenja.GostiZaStolom")
                                                    .Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik.Stolovi.GostiZaStolom")
                                                    .SingleOrDefault(d => d.Id == idDogadjaja);

                    //if (dogadjaj.NerasporedjeniGosti == null)
                    //{
                    //    dogadjaj.NerasporedjeniGosti = new List<Gost>();
                    //}
                    if (dogadjaj.RasporedSedenja == null || dogadjaj.RasporedSedenja.Count() == 0)
                    {
                        List<KapacitetStola> rasporedSedenja = new List<KapacitetStola>();
                        foreach (Zadatak zad in dogadjaj.Zadaci)
                        {
                            if (zad.Tip == Zadatak.TipZadatka.GLAVNI)
                            {
                                List<KapacitetStola> kapacitetiStolovaUPonudi = zad.IzabraniPredlog.Ponuda.Saradnik.Stolovi;
                                foreach (KapacitetStola ks in kapacitetiStolovaUPonudi)
                                {
                                    KapacitetStola kapacitetSaGostima = new KapacitetStola();
                                    kapacitetSaGostima.Kapacitet = ks.Kapacitet;
                                    kapacitetSaGostima.Naziv = ks.Naziv;
                                    kapacitetSaGostima.GostiZaStolom = new List<Gost>();
                                    rasporedSedenja.Add(kapacitetSaGostima);
                                }

                                break;
                            }
                        }

                        dogadjaj.RasporedSedenja = rasporedSedenja;
                    }

                    db.SaveChanges();
                    _navigationStore.CurrentViewModel = new RasporedSedenjaViewModel(_navigationStore, _navigationStore.CurrentViewModel, dogadjaj, window, Organizovan);
                }
            }
            else
            {
                // ovo znaci da je vec menjan raspored sedenja u ovoj "sesiji" klijenta

                _navigationStore.CurrentViewModel = new RasporedSedenjaViewModel(_navigationStore, _navigationStore.CurrentViewModel, tempDogadjaj, window, Organizovan,
                                                                                 tempNerasporedjeniGosti.DeepClone(), tempRasporedSedenja.DeepClone());
            }
        }

        private ICommand _prihvatiPredlogCommand;
        public ICommand PrihvatiPredlogCommand
        {
            get
            {
                if (_prihvatiPredlogCommand == null)
                    _prihvatiPredlogCommand = new RelayCommand(window => PrihvatiPredlog((Window) window));
                return _prihvatiPredlogCommand;
            }
        }

        private void PrihvatiPredlog(Window window)
        {
            Dialog dialog = new Dialog();
            DialogViewModel dialogModel = new DialogViewModel();
            dialogModel.Message = $"Da li ste sigurni da želite da prihvatite predlog?";
            dialog.DataContext = dialogModel;
            dialog.Owner = window;
            dialog.ShowDialog();

            if (dialogModel.odgovor.Equals("Ne"))
            {
                return;
            }

            using (var db = new DatabaseContext())
            {
                int idDogadjaja = Zadaci.First().Dogadjaj.Id;
                Dogadjaj dog = db.Dogadjaji.Include("Zadaci.IzabraniPredlog")
                                           .Include("PrihvaceniGlavniPredlog")
                                           .Include("PrihvaceniDodatniPredlozi")
                                           .Include("RasporedSedenja")
                                           .SingleOrDefault(d => d.Id == idDogadjaja);
                dog.StatusEnum = Dogadjaj.STATUS_DOGADJAJA.ORGANIZOVAN;
                dog.PrihvaceniGlavniPredlog = dog.Zadaci.SingleOrDefault(z => z.Tip == Zadatak.TipZadatka.GLAVNI).IzabraniPredlog;
                if (dog.PrihvaceniDodatniPredlozi == null)
                {
                    dog.PrihvaceniDodatniPredlozi = new List<Predlog>();
                }
                foreach (Zadatak zad in dog.Zadaci)
                {
                    if (zad.Tip == Zadatak.TipZadatka.DODATNI)
                    {
                        dog.PrihvaceniDodatniPredlozi.Add(zad.IzabraniPredlog);
                    }
                }
                
                //if (tempNerasporedjeniGosti != null)
                //{
                //    List<Gost> nerasporedjeniGostiZaDB = new List<Gost>();
                //    foreach (Gost g in tempNerasporedjeniGosti)
                //    {
                //        g.Id = null;
                //        nerasporedjeniGostiZaDB.Add(g);
                //    }

                //    dog.NerasporedjeniGosti = nerasporedjeniGostiZaDB;

                //    List<KapacitetStola> rasporedSedenjaZaDB = new List<KapacitetStola>();
                //    foreach (KapacitetStola ks in tempRasporedSedenja)
                //    {
                //        ks.Id = null;
                //        List<Gost> gostiZaStolomZaDB = new List<Gost>();
                //        foreach (Gost g in ks.GostiZaStolom)
                //        {
                //            g.Id = null;
                //            gostiZaStolomZaDB.Add(g);
                //        }
                //        ks.GostiZaStolom = gostiZaStolomZaDB;
                //        rasporedSedenjaZaDB.Add(ks);
                //    }
                //    dog.RasporedSedenja = rasporedSedenjaZaDB;
                //}

                db.SaveChanges();
            }

            SuccessOrErrorDialog successDialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel successDialogModel = new SuccessOrErrorDialogViewModel();
            successDialogModel.Message = $"Uspešno ste prihvatili predlog organizatora. ";
            successDialog.DataContext = successDialogModel;
            successDialog.Owner = window;
            successDialog.ShowDialog();

            Povratak();
        }

        private ICommand _zatraziIzmeneCommand;
        public ICommand ZatraziIzmeneCommand
        {
            get
            {
                if (_zatraziIzmeneCommand == null)
                    _zatraziIzmeneCommand = new RelayCommand(window => ZatraziIzmene((Window) window));
                return _zatraziIzmeneCommand;
            }
        }

        private void ZatraziIzmene(Window window)
        {
            Dialog dialog = new Dialog();
            DialogViewModel dialogModel = new DialogViewModel();
            dialogModel.Message = $"Da li ste sigurni da želite da tražite izmenu predloga?";
            dialog.DataContext = dialogModel;
            dialog.Owner = window;
            dialog.ShowDialog();

            if (dialogModel.odgovor.Equals("Ne"))
            {
                return;
            }

            int idDogadjaja = Zadaci.First().Dogadjaj.Id;
            using (var db = new DatabaseContext())
            {
                Dogadjaj dog = db.Dogadjaji.Include("RasporedSedenja")
                                           .Include("NerasporedjeniGosti")
                                           .SingleOrDefault(d => d.Id == idDogadjaja);

                if (tempNerasporedjeniGosti != null)
                {
                    List<Gost> nerasporedjeniGostiZaDB = new List<Gost>();
                    foreach (Gost g in tempNerasporedjeniGosti)
                    {
                        g.Id = null;
                        nerasporedjeniGostiZaDB.Add(g);
                    }

                    dog.NerasporedjeniGosti = nerasporedjeniGostiZaDB;

                    List<KapacitetStola> rasporedSedenjaZaDB = new List<KapacitetStola>();
                    foreach (KapacitetStola ks in tempRasporedSedenja)
                    {
                        ks.Id = null;
                        List<Gost> gostiZaStolomZaDB = new List<Gost>();
                        foreach (Gost g in ks.GostiZaStolom)
                        {
                            g.Id = null;
                            gostiZaStolomZaDB.Add(g);
                        }
                        ks.GostiZaStolom = gostiZaStolomZaDB;
                        rasporedSedenjaZaDB.Add(ks);
                    }
                    dog.RasporedSedenja = rasporedSedenjaZaDB;
                }

                dog.StatusEnum = Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR;

                db.SaveChanges();
            }

            SuccessOrErrorDialog successDialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel successDialogModel = new SuccessOrErrorDialogViewModel();
            successDialogModel.Message = $"Uspešno ste zatražili izmenu predloga organizatoru. Otvoriće Vam se prozor za komunikaciju sa organizatorom" +
                                         $" u kom možete navesti razloge za izmenu predloga.";
            successDialog.DataContext = successDialogModel;
            successDialog.Owner = window;
            successDialog.ShowDialog();

            // otvoriti poruke
            Komunikacija porukeDialog = new Komunikacija();
            KomunikacijaViewModel porukeViewModel = new KomunikacijaViewModel(false);
            porukeViewModel._idDogadjaja = idDogadjaja;
            porukeViewModel.Komentari = KomentarService.getKomentareZaDogadjaj(idDogadjaja);
            porukeDialog.DataContext = porukeViewModel;
            porukeDialog.Owner = window;
            porukeDialog.ShowDialog();

            Povratak();
        }

        private readonly IKomentarService KomentarService = new KomentarService();

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
            _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore,_navigationStore.CurrentViewModel, IsKlijent);
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

        public (double, double) GetLocationFromAddressAsync()
        {
            string drzava = Predlog.Ponuda.Saradnik.Adresa.Drzava;
            string grad = Predlog.Ponuda.Saradnik.Adresa.Grad;
            string ulica = Predlog.Ponuda.Saradnik.Adresa.Ulica;
            int broj = Predlog.Ponuda.Saradnik.Adresa.Broj;

            double lat = -1, lon = -1;

            string address = $"{drzava} {grad} {ulica} {broj}";
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
    }
}
