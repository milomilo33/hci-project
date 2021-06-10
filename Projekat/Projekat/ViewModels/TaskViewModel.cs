using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<Zadatak> _zadaci;
        private readonly IZadatakService ZadatakServce = new ZadatakService();
        private readonly IPonudaService PonudaService = new PonudaService();
        private ICommand _addCommand;
        private ICommand _povratakCommand;
        private ICommand _izmenaCommand;
        private ICommand _komentarCommand;
        private ICommand _prihvatiCommand;
        private ICommand _sendClientCommand;
        private ICommand _deleteCommand;

        public int _idDogadjaja;
        public NavigationStore _navigationStore;
        public TaskViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public int IdDogadjaja
        {
            get => _idDogadjaja;
            set
            {
                _idDogadjaja = value;
                OnPropertyChanged(nameof(IdDogadjaja));
            }
        }
        public ObservableCollection<Zadatak> Zadaci
        {
            get { return _zadaci; }
            set
            {
                _zadaci = value;
                OnPropertyChanged(nameof(Zadaci));
            }
        }

        private Zadatak _zadatak;
        public Zadatak SelectedZadatak
        {
            get => _zadatak;
            set
            {
                _zadatak = value;
                OnPropertyChanged(nameof(SelectedZadatak));
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(window => Add((Window) window));
                return _addCommand;
            }
        }
        public void Add(Window window)
        {
            string s = "";
            using (var db = new DatabaseContext())
            {
                var d = db.Dogadjaji.Find(IdDogadjaja);
                s = d.Status;

            }
            if (s.Equals("Čeka se odgovor klijenta"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Nije moguće kreirati novi zadatak, čeka se odgovor klijenta!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else if (s.Equals("Organizovan"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Nije moguće kreirati novi zadatak, događaj je već organizovan!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else
            {

                CreateTask view = new CreateTask(this);
                KreiranjeZadatkaViewModel viewModel = new KreiranjeZadatkaViewModel(_idDogadjaja);
                view.DataContext = viewModel;
                view.Owner = window;
                view.ShowDialog();

            }
        }

        public void refresh()
        {
            Zadaci = ZadatakServce.sviZadaciZaDogadjaj(_idDogadjaja);
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

        public void Povratak()
        {

            _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore, _navigationStore.CurrentViewModel);


        }

        public ICommand IzmenaCommand
        {
            get
            {
                if (_izmenaCommand == null)
                    _izmenaCommand = new RelayCommand(window => Izmena((Window)window));
                return _izmenaCommand;
            }
        }
        public ICommand KomentarCommand
        {
            get
            {
                if (_komentarCommand == null)
                    _komentarCommand = new RelayCommand(window => Komentar((Window)window));
                return _komentarCommand;
            }
        }

        public ICommand PrihvatiCommand
        {
            get
            {
                if (_prihvatiCommand == null)
                    _prihvatiCommand = new RelayCommand(window => Prihvati((Window)window));
                return _prihvatiCommand;
            }
        }
        public ICommand SendClientCommand
        {
            get
            {
                if (_sendClientCommand == null)
                    _sendClientCommand = new RelayCommand(window => Send((Window) window));
                return _sendClientCommand;
            }
        }

        public void Send(Window window)
        {
            string s = "";
            using (var db = new DatabaseContext())
            {
                var d = db.Dogadjaji.Find(IdDogadjaja);
                s = d.Status;
               
            }
            if (s.Equals("Čeka se odgovor klijenta"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Predlozi su već poslati klijentu!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else if (s.Equals("Organizovan"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Događaj je već organizovan!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else
            {

                if (ZadatakServce.proveraGlavniZadatak(IdDogadjaja))
                {
                    using (var db = new DatabaseContext())
                    {
                        var d = db.Dogadjaji.Find(IdDogadjaja);
                        d.StatusEnum = Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_KLIJENT;
                        db.SaveChanges();
                    }
                    SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                    dialogModel.IsError = false;
                    dialogModel.Message = "Uspešno ste poslali predloge klijentu!";
                    dialog.DataContext = dialogModel;
                    dialog.Owner = window;
                    dialog.ShowDialog();
                }
                else
                {
                    SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                    dialogModel.IsError = true;
                    dialogModel.Message = "Morate da prihvatite ponude za sve zadatke!";
                    dialog.DataContext = dialogModel;
                    dialog.Owner = window;
                    dialog.ShowDialog();
                }
            }
        }
        public void Komentar(Window window)
        {
            Komunikacija view = new Komunikacija();
            KomunikacijaViewModel viewModel = new KomunikacijaViewModel();
            viewModel.IdZadatka = SelectedZadatak.Id;
            viewModel.loadKomentari();
            view.DataContext = viewModel;
            view.Owner = window;
            view.Show();
        }
        public void Izmena(Window window)
        {
            string status = "";
            using (var db = new DatabaseContext())
            {
                var d = db.Dogadjaji.Find(IdDogadjaja);
                status = d.Status;
                
            }
            if (status.Equals("Čeka se odgovor klijenta"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Ne možete da izmenite zadatak, čeka se odgovor klijenta!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }else if (status.Equals("Organizovan"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Ne možete da izmenite zadatak, događaj je organizovan!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }

            else
            {

                IzmenaZadatkaView view = new IzmenaZadatkaView(this);
                IzmenaZadatkaViewModel viewModel = new IzmenaZadatkaViewModel();
                viewModel._navigationStore = _navigationStore;
                viewModel.IdDogadjaja = IdDogadjaja;
                viewModel.zadatak = SelectedZadatak;
                viewModel.Naziv = SelectedZadatak.Naziv;
                viewModel.Opis = SelectedZadatak.Opis;
                view.DataContext = viewModel;
                view.Owner = window;
                view.ShowDialog();
            }
        }

        public void Prihvati(Window window)
        {
            string status = "";
            using (var db = new DatabaseContext())
            {
                var d = db.Dogadjaji.Find(IdDogadjaja);
                status = d.Status;

            }
            if (status.Equals("Čeka se odgovor klijenta"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Ne možete da prihvatate ponude, čeka se odgovor klijenta!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else if (status.Equals("Organizovan"))
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Ne možete da prihvatate ponude, događaj je organizovan!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                var zad = ZadatakServce.getZadatakSaPredlogom(SelectedZadatak.Id);
                if (zad.IzabraniPredlog != null && status.Equals("Čeka se odgovor klijenta") || zad.IzabraniPredlog != null && status.Equals("Organizovan"))
                {
                    SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                    dialogModel.IsError = true;
                    dialogModel.Message = "Već ste prihvatili ponude za ovaj zadatak!";
                    dialog.DataContext = dialogModel;
                    dialog.Owner = window;
                    dialog.ShowDialog();
                }
                else
                {
                    PredloziZaZadatak view = new PredloziZaZadatak();
                    PredloziZaZadatakViewModel viewModel = new PredloziZaZadatakViewModel();
                    viewModel.Ponude = PonudaService.svePonudeZaZadatak(SelectedZadatak.Id);
                    viewModel.IdZadatka = SelectedZadatak.Id;
                    view.DataContext = viewModel;
                    view.Owner = window;
                    view.ShowDialog();
                }
            }

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

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(window => Delete((Window)window));
                return _deleteCommand;
            }
        }

        public void Delete(Window window)
        {
            
            
            if(SelectedZadatak == null)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Morate selektovati zadatak kako biste ga obrisali!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else {
                using (var db = new DatabaseContext())
                {
                    var dogadjaj = db.Dogadjaji.Find(IdDogadjaja);
                    if (dogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_KLIJENT || dogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.ORGANIZOVAN)
                    {
                        SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                        SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                        dialogModel.IsError = true;
                        dialogModel.Message = "Nije moguće obrisati izabrani zadatak!";
                        dialog.DataContext = dialogModel;
                        dialog.Owner = window;
                        dialog.ShowDialog();
                    }
                    else
                    {
                        var zadatak = db.Zadaci.Find(SelectedZadatak.Id);
                        if (zadatak.Tip == Zadatak.TipZadatka.GLAVNI)
                        {
                            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                            dialogModel.IsError = true;
                            dialogModel.Message = "Nije moguće obrisati glavni zadatak!";
                            dialog.DataContext = dialogModel;
                            dialog.Owner = window;
                            dialog.ShowDialog();
                        }
                        else
                        {
                            db.Zadaci.Remove(zadatak);
                            db.SaveChanges();
                            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                            dialogModel.IsError = false;
                            dialogModel.Message = "Uspešno ste obrisali izabrani zadatak!";
                            dialog.DataContext = dialogModel;
                            dialog.Owner = window;
                            dialog.ShowDialog();
                            refresh();
                        }
                    }

                }
            }
        }
        private ICommand _odjavaCommand;
        public ICommand OdjavaCommand
        {
            get
            {
                if (_odjavaCommand == null)
                    _odjavaCommand = new RelayCommand(_odjavaCommand => Odjava());
                return _odjavaCommand;
            }
        }

        public void Odjava()
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);

        }

    }
}
