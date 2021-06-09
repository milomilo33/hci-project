using Projekat.Commands;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    internal class EventListViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;
        private readonly IDogadjajService DogadjajService = new DogadjajService();


        private Dogadjaj _selectedDogadjaj;
        public Dogadjaj SelectedDogadjaj
        {
            get { return _selectedDogadjaj; }
            set 
            {
                _selectedDogadjaj = value;
                OnPropertyChanged(nameof(SelectedDogadjaj));
            }
        }

        public ObservableCollection<Dogadjaj> Dogadjaji { get; set; }

        public EventListViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            Dogadjaji = DogadjajService.sviDogadjaji();
        }

        public void LoadEvents(IEnumerable<Dogadjaj> dogadjaji)
        {
            Dogadjaji = new ObservableCollection<Dogadjaj>(dogadjaji);
            OnPropertyChanged(nameof(Dogadjaj));
        }

        private ICommand _pregledPonudaCommand;
        public ICommand PregledPonudaCommand
        {
            get
            {
                if (_pregledPonudaCommand == null)
                    _pregledPonudaCommand = new RelayCommand(_pregledPonudaCommand => PregledajPonude());
                return _pregledPonudaCommand;
            }
        }

        private ICommand _acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(_acceptCommand => Accept());
                return _acceptCommand;
            }
        }

        public void Accept()
        {
            var dd = Dogadjaji;
            foreach (var d in dd)
            {
                if (d.Id == _selectedDogadjaj.Id)
                {
                    d.Status = "Preuzeto";
                    DogadjajService.updateStatus(d);
                }
            }
            Dogadjaji = null;
            Dogadjaji = dd;


        }
        private void PregledajPonude()
        {
            _navigationStore.CurrentViewModel = new PregledPonudaViewModel(_navigationStore);
        }

        private ICommand _detailsCommand;
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
            detailsModel.Organizator = SelectedDogadjaj.Organizator.Ime + " " + SelectedDogadjaj.Organizator.Prezime;
            detailsModel.DatumOdrzavanja = SelectedDogadjaj.DatumOdrzavanja.ToString("dd/MM/yyyy HH:mm");
            detailsModel.DodatniZahtevi = SelectedDogadjaj.DodatniZahtevi;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.Show();

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
    }
}