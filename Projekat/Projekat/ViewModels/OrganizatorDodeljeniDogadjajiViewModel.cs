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
    public class OrganizatorDodeljeniDogadjajiViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewModelBase _previousModel;
        public ObservableCollection<Dogadjaj> dogadjaji;
        private ICommand _taskCommand;
        private readonly IDogadjajService DogadjajService = new DogadjajService();
        private readonly IZadatakService ZadatakServce = new ZadatakService();
        private ICommand _detailsCommand;
        private ICommand _povratakCommand;
        
        private Korisnik k;

        public ObservableCollection<Dogadjaj> Dogadjaji
        {
            get { return dogadjaji; }
            set
            {
                dogadjaji = value;
                OnPropertyChanged(nameof(Dogadjaji));
            }
        }
        public OrganizatorDodeljeniDogadjajiViewModel(NavigationStore navigationStore,ViewModelBase viewModelBase, bool isKlijent = false)
        {
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            k = korisnikStore.TrenutniKorisnik;

            if (isKlijent)
            {
                Dogadjaji = DogadjajService.sviDogadjajiZaKlijenta(k.Email);
            }
            else
            {
                Dogadjaji = DogadjajService.sviDogadjajiZaOrganizatora(k.Email);
            }

            _navigationStore = navigationStore;
            _previousModel = viewModelBase;
            IsKlijent = isKlijent;
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

        private Dogadjaj _dogadjaj;
        public Dogadjaj SelectedDogadjaj
        {
            get => _dogadjaj;
            set
            {
                _dogadjaj = value;
                OnPropertyChanged(nameof(SelectedDogadjaj));
            }
        }
        public ICommand TaskCommand
        {
            get
            {
                if (_taskCommand == null)
                    _taskCommand = new RelayCommand(_taskCommand => Tasks());
                return _taskCommand;
            }
        }
        public void Tasks()
        {
            ObservableCollection<Zadatak> zadaci = ZadatakServce.sviZadaciZaDogadjaj(SelectedDogadjaj.Id);
            TaskViewModel tvm = new TaskViewModel(_navigationStore);
            tvm.Zadaci = zadaci;
            
            tvm.IdDogadjaja = SelectedDogadjaj.Id;
            _navigationStore.CurrentViewModel = tvm;
        }

        private ICommand _predlogOrganizatoraCommand;
        public ICommand PredlogOrganizatoraCommand
        {
            get
            {
                if (_predlogOrganizatoraCommand == null)
                    _predlogOrganizatoraCommand = new RelayCommand(window => OtvoriPredlogOrganizatora((Window) window));
                return _predlogOrganizatoraCommand;
            }
        }
        public void OtvoriPredlogOrganizatora(Window window)
        {
            if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Čeka se predlog organizatora.";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.NEDODELJEN)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Nijedan organizator nije još prihvatio događaj.";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                // ucitavanje i otvaranje stranice predloga

                using (var db = new DatabaseContext())
                {
                    //List<Dogadjaj> dogadjajiZaKlijenta = db.Klijenti.Include("Dogadjaji").Include("Dogadjaji.Organizator").SingleOrDefault(k => k.Email.Equals(email)).Dogadjaji;
                    //if (dogadjajiZaKlijenta == null)
                    //{
                    //    dogadjajiZaKlijenta = new List<Dogadjaj>();
                    //}
                    List<Zadatak> zadaci = db.Dogadjaji.Include("Zadaci").
                                                   Include("Zadaci.IzabraniPredlog").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik.Adresa").
                                                   SingleOrDefault(d => d.Id == SelectedDogadjaj.Id).
                                                   Zadaci; 
                    Predlog predlog = zadaci.Find(z => z.Tip == Zadatak.TipZadatka.GLAVNI).IzabraniPredlog;

                    bool organizovan = false;
                    if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.ORGANIZOVAN)
                    {
                        organizovan = true;
                    }
                    PregledPredlogaViewModel pregledPredlogaViewModel = new PregledPredlogaViewModel(_navigationStore, predlog, zadaci, organizovan);
                    _navigationStore.CurrentViewModel = pregledPredlogaViewModel;
                }
            }
        }

        private ICommand _pregledPredlogaCommand;
        public ICommand PregledPredlogaCommand
        {
            get
            {
                if (_pregledPredlogaCommand == null)
                    _pregledPredlogaCommand = new RelayCommand(window => OtvoriPregledPredloga((Window)window));
                return _pregledPredlogaCommand;
            }
        }
        public void OtvoriPregledPredloga(Window window)
        {
            if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_KLIJENT)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Čeka se odgovor klijenta na predlog.";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                // ucitavanje i otvaranje stranice predloga

                using (var db = new DatabaseContext())
                {
                    List<Zadatak> zadaci = db.Dogadjaji.Include("Zadaci").
                                                   Include("Zadaci.IzabraniPredlog").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik.Adresa").
                                                   SingleOrDefault(d => d.Id == SelectedDogadjaj.Id).
                                                   Zadaci;
                    Predlog predlog = zadaci.Find(z => z.Tip == Zadatak.TipZadatka.GLAVNI).IzabraniPredlog;
                    if (predlog == null)
                    {
                        SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                        SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                        dialogModel.IsError = true;
                        dialogModel.Message = "Niste izabrali ponudu za ovaj događaj!";
                        dialog.DataContext = dialogModel;
                        dialog.Owner = window;
                        dialog.ShowDialog();

                        return;
                    }

                    bool organizovan = false;
                    if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.ORGANIZOVAN)
                    {
                        organizovan = true;
                    }
                    PregledPredlogaViewModel pregledPredlogaViewModel = new PregledPredlogaViewModel(_navigationStore, predlog, zadaci, organizovan);
                    _navigationStore.CurrentViewModel = pregledPredlogaViewModel;
                }
            }
        }

        private ICommand _otvoriPorukeCommand;
        public ICommand OtvoriPorukeCommand
        {
            get
            {
                if (_otvoriPorukeCommand == null)
                    _otvoriPorukeCommand = new RelayCommand(window => OtvoriPoruke((Window) window));
                return _otvoriPorukeCommand;
            }
        }

        private void OtvoriPoruke(Window window)
        {
            Komunikacija porukeDialog = new Komunikacija();
            KomunikacijaViewModel porukeViewModel = new KomunikacijaViewModel(false);
            porukeViewModel._idDogadjaja = SelectedDogadjaj.Id;
            porukeViewModel.Komentari = KomentarService.getKomentareZaDogadjaj(SelectedDogadjaj.Id);
            porukeDialog.DataContext = porukeViewModel;
            porukeDialog.Owner = window;
            porukeDialog.ShowDialog();
        }

        private readonly IKomentarService KomentarService = new KomentarService();

        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(window => DetailsEvent((Window) window));
                return _detailsCommand;
            }
        }
        public void DetailsEvent(Window window)
        {
            Details details = new Details();
            DetailsViewModel detailsModel = new DetailsViewModel();
            detailsModel.Vrsta = SelectedDogadjaj.VrstaProslave;
            detailsModel.Budzet = SelectedDogadjaj.Budzet;
            detailsModel.Tema = SelectedDogadjaj.Tema;
            if (SelectedDogadjaj.Organizator != null)
            {
                detailsModel.Organizator = SelectedDogadjaj.Organizator.Ime + " " + SelectedDogadjaj.Organizator.Prezime;
            }
            detailsModel.DatumOdrzavanja = SelectedDogadjaj.DatumOdrzavanja.ToString("dd.MM.yyyy");
            detailsModel.DodatniZahtevi = SelectedDogadjaj.DodatniZahtevi;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            detailsModel.BrojGostiju = SelectedDogadjaj.BrojGostiju;
            details.DataContext = detailsModel;
            details.ShowDialog();

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
