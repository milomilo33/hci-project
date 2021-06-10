using Projekat.Commands;
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
    public class OrganizatorEventTableViewModel : ViewModelBase
    {
        public ObservableCollection<Dogadjaj> dogadjaji;
        private readonly NavigationStore _navigationStore;
        private ICommand _acceptCommand;
        private readonly IDogadjajService DogadjajService = new DogadjajService();
        private ICommand _detailsCommand;

        public ObservableCollection<Dogadjaj> Dogadjaji
        {
            get { return dogadjaji; }
            set
            {
                dogadjaji = value;
                OnPropertyChanged(nameof(Dogadjaji));
            }
        }
        public OrganizatorEventTableViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            Dogadjaji = DogadjajService.sviDogadjaji();

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
        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(window => Accept((Window) window));
                return _acceptCommand;
            }
        }
        public void Accept(Window window)
        {
         
            if (SelectedDogadjaj == null)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Morate selektovati događaj iz tabele kako biste ga preuzeli!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                var dd = dogadjaji;
                foreach (var d in dd)
                {
                    if (d.Id == SelectedDogadjaj.Id && d.Status.Equals("Nema dodeljenog organizatora"))
                    {
                        d.Status = "Čeka se odgovor organizatora";
                        d.StatusEnum = Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR;
                        DogadjajService.updateStatus(d);
                        SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                        SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                        dialogModel.IsError = false;
                        dialogModel.Message = "Uspešno prihvaćen događaj!";
                        dialog.DataContext = dialogModel;
                        dialog.Owner = window;
                        dialog.ShowDialog();
                    }
                    else if (d.Id == SelectedDogadjaj.Id && d.Status != "Nema dodeljenog organizatora")
                    {
                        SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                        SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                        dialogModel.IsError = true;
                        dialogModel.Message = "Događaj je već preuzet!";
                        dialog.DataContext = dialogModel;
                        dialog.Owner = window;
                        dialog.ShowDialog();

                    }
                    else
                    {

                    }
                }
                Dogadjaji = null;
                Dogadjaji = dd;
            }


        }
        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(_acceptCommand => DetailsEvent());
                return _detailsCommand;
            }
        }
        public void DetailsEvent()
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
            detailsModel.DatumOdrzavanja = SelectedDogadjaj.DatumOdrzavanja.ToString("dd.MM.yyyy. HH:mm");
            detailsModel.DodatniZahtevi = SelectedDogadjaj.DodatniZahtevi;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.ShowDialog();

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
            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
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
