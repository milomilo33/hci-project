using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class RasporedSedenjaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ViewModelBase _previousViewModel;

        private Dogadjaj _dogadjaj;

        public Window _window;

        public RasporedSedenjaViewModel(NavigationStore navigationStore, ViewModelBase previousViewModel, Dogadjaj dogadjaj, Window window,
                                        ObservableCollection<Gost> nerasporedjeniGosti = null, ObservableCollection<KapacitetStola> rasporedSedenja = null)
        {
            _navigationStore = navigationStore;
            _previousViewModel = previousViewModel;
            _dogadjaj = dogadjaj;

            if (dogadjaj.NerasporedjeniGosti == null)
            {
                dogadjaj.NerasporedjeniGosti = new List<Gost>();
            }

            if (nerasporedjeniGosti == null && rasporedSedenja == null)
            {
                NerasporedjeniGosti = new ObservableCollection<Gost>(dogadjaj.NerasporedjeniGosti);
                RasporedSedenja = new ObservableCollection<KapacitetStola>(dogadjaj.RasporedSedenja);
            }
            else
            {
                NerasporedjeniGosti = nerasporedjeniGosti;
                RasporedSedenja = rasporedSedenja;
            }

            _window = window;
        }

        private ObservableCollection<Gost> _nerasporedjeniGosti;
        public ObservableCollection<Gost> NerasporedjeniGosti
        {
            get { return _nerasporedjeniGosti; }
            set
            {
                _nerasporedjeniGosti = value;
                OnPropertyChanged(nameof(NerasporedjeniGosti));
            }
        }

        private ObservableCollection<KapacitetStola> _rasporedSedenja;
        public ObservableCollection<KapacitetStola> RasporedSedenja
        {
            get { return _rasporedSedenja; }
            set
            {
                _rasporedSedenja = value;
                OnPropertyChanged(nameof(RasporedSedenja));
            }
        }

        private ICommand _dodajNerasporedjenogGostaCommand;
        public ICommand DodajNerasporedjenogGostaCommand
        {
            get
            {
                if (_dodajNerasporedjenogGostaCommand == null)
                    _dodajNerasporedjenogGostaCommand = new RelayCommand(window => DodajNerasporedjenogGosta((Window) window));
                return _dodajNerasporedjenogGostaCommand;
            }
        }

        private void DodajNerasporedjenogGosta(Window window)
        {
            int trenutniBrojGostiju = NerasporedjeniGosti.Count();
            foreach (KapacitetStola ks in RasporedSedenja)
            {
                trenutniBrojGostiju += ks.GostiZaStolom.Count();
            }

            if (trenutniBrojGostiju >= _dogadjaj.BrojGostiju)
            {
                SuccessOrErrorDialog dialog3 = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel3 = new SuccessOrErrorDialogViewModel();
                dialogModel3.Message = $"Već ste dodali maksimalan broj gostiju za ovaj događaja!";
                dialog3.DataContext = dialogModel3;
                dialogModel3.IsError = true;
                dialog3.Owner = window;
                dialog3.ShowDialog();
            }
            else
            {
                DodajGostaDialogView dialog = new DodajGostaDialogView();
                DodajGostaDialogViewModel dialogModel = new DodajGostaDialogViewModel();
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

                if (dialogModel.TrebaDodatiGosta)
                {
                    Gost noviGost = new Gost();
                    // dodati proveru i za rasporedjene goste (dodato?)
                    foreach (Gost g in NerasporedjeniGosti)
                    {
                        if (g.ImeIPrezime.Equals(dialogModel.ImeIPrezimeGosta))
                        {
                            SuccessOrErrorDialog dialog3 = new SuccessOrErrorDialog();
                            SuccessOrErrorDialogViewModel dialogModel3 = new SuccessOrErrorDialogViewModel();
                            dialogModel3.Message = $"Gost {dialogModel.ImeIPrezimeGosta} već postoji!";
                            dialog3.DataContext = dialogModel3;
                            dialogModel3.IsError = true;
                            dialog3.Owner = window;
                            dialog3.ShowDialog();
                            return;
                        }
                    }
                    foreach (KapacitetStola ks in RasporedSedenja)
                    {
                        foreach (Gost g in ks.GostiZaStolom)
                        {
                            if (g.ImeIPrezime.Equals(dialogModel.ImeIPrezimeGosta))
                            {
                                SuccessOrErrorDialog dialog3 = new SuccessOrErrorDialog();
                                SuccessOrErrorDialogViewModel dialogModel3 = new SuccessOrErrorDialogViewModel();
                                dialogModel3.Message = $"Gost {dialogModel.ImeIPrezimeGosta} već postoji!";
                                dialog3.DataContext = dialogModel3;
                                dialogModel3.IsError = true;
                                dialog3.Owner = window;
                                dialog3.ShowDialog();
                                return;
                            }
                        }
                    }
                    noviGost.ImeIPrezime = dialogModel.ImeIPrezimeGosta;
                    NerasporedjeniGosti.Add(noviGost);

                    SuccessOrErrorDialog dialog2 = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel2 = new SuccessOrErrorDialogViewModel();
                    dialogModel2.Message = $"Uspešno ste dodali gosta {dialogModel.ImeIPrezimeGosta} u događaj.";
                    dialog2.DataContext = dialogModel2;
                    dialog2.Owner = window;
                    dialog2.ShowDialog();
                    return;
                }
            }
        }

        private ICommand _obrisiNerasporedjenogGostaCommand;
        public ICommand ObrisiNerasporedjenogGostaCommand
        {
            get
            {
                if (_obrisiNerasporedjenogGostaCommand == null)
                    _obrisiNerasporedjenogGostaCommand = new RelayCommand(parameters => ObrisiNerasporedjenogGosta((object[]) parameters));
                return _obrisiNerasporedjenogGostaCommand;
            }
        }

        // brise i nerasporedjene i rasporedjene
        private void ObrisiNerasporedjenogGosta(object[] parameters)
        {
            string imeIPrezimeGosta = (string)parameters[0];
            Gost gost = NerasporedjeniGosti.SingleOrDefault(g => g.ImeIPrezime.Equals(imeIPrezimeGosta));
            bool shouldUpdate = false;
            if (!NerasporedjeniGosti.Remove(gost))
            {
                foreach (KapacitetStola ks in RasporedSedenja)
                {
                    gost = ks.GostiZaStolom.SingleOrDefault(g => g.ImeIPrezime.Equals(imeIPrezimeGosta));
                    if (ks.GostiZaStolom.Remove(gost))
                    {
                        shouldUpdate = true;
                        break;
                    }
                }
            }

            if (shouldUpdate)
            {
                var backup = RasporedSedenja;
                RasporedSedenja = null;
                RasporedSedenja = backup;
            }

            Window window = (Window)parameters[1];

            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
            dialogModel.Message = $"Uspešno ste obrisali gosta {imeIPrezimeGosta} iz događaja.";
            dialog.DataContext = dialogModel;
            dialog.Owner = window;
            dialog.ShowDialog();
        }

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
            _navigationStore.CurrentViewModel = _previousViewModel;
        }

        private ICommand _potvrdiIzmeneCommand;

        public ICommand PotvrdiIzmeneCommand
        {
            get
            {
                if (_potvrdiIzmeneCommand == null)
                    _potvrdiIzmeneCommand = new RelayCommand(_potvrdiIzmeneCommand => PotvrdiIzmene());
                return _potvrdiIzmeneCommand;
            }
        }

        public void PotvrdiIzmene()
        {
            // dijalog za potvrdu
            Dialog dialog = new Dialog();
            DialogViewModel dialogModel = new DialogViewModel();
            dialogModel.Message = $"Da li ste sigurni da želite da izmenite raspored sedenja?";
            dialog.DataContext = dialogModel;
            dialog.Owner = _window;
            dialog.ShowDialog();
            
            if (dialogModel.odgovor.Equals("Ne"))
            {
                return;
            }

            if (KorisnikStore.Instance.TrenutniKorisnik is Organizator)
            {
                using (var db = new DatabaseContext())
                {
                    List<Gost> nerasporedjeniGostiZaDB = new List<Gost>();
                    foreach (Gost g in NerasporedjeniGosti)
                    {
                        g.Id = null;
                        nerasporedjeniGostiZaDB.Add(g);
                    }

                    Dogadjaj dog = db.Dogadjaji.Include("NerasporedjeniGosti")
                                               .Include("RasporedSedenja.GostiZaStolom")
                                               .SingleOrDefault(d => d.Id == _dogadjaj.Id);
                    dog.NerasporedjeniGosti = nerasporedjeniGostiZaDB;

                    List<KapacitetStola> rasporedSedenjaZaDB = new List<KapacitetStola>();
                    foreach (KapacitetStola ks in RasporedSedenja)
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

                    db.SaveChanges();
                }
            }
            else
            {
                ((PregledPredlogaViewModel)_previousViewModel).tempNerasporedjeniGosti = NerasporedjeniGosti;
                ((PregledPredlogaViewModel)_previousViewModel).tempRasporedSedenja = RasporedSedenja;
                ((PregledPredlogaViewModel)_previousViewModel).tempDogadjaj = _dogadjaj;
            }

            // success dijalog
            SuccessOrErrorDialog successDialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel successDialogModel = new SuccessOrErrorDialogViewModel();
            successDialogModel.Message = $"Uspešno ste izmenili raspored sedenja. ";
            if (KorisnikStore.Instance.TrenutniKorisnik is Klijent)
            {
                successDialogModel.Message += "Da biste potvrdili ovaj raspored sedenja za događaj, nakon ovoga zatražite izmenu predloga organizatoru klikom" +
                                              "na dugme u donjem desnom uglu na narednom prozoru.";
            }
            successDialog.DataContext = successDialogModel;
            successDialog.Owner = _window;
            successDialog.ShowDialog();

            _navigationStore.CurrentViewModel = _previousViewModel;
        }
    }
}
