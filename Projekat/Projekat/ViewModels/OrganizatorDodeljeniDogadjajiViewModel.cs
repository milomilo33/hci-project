﻿using Projekat.Commands;
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
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class OrganizatorDodeljeniDogadjajiViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
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
        public OrganizatorDodeljeniDogadjajiViewModel(NavigationStore navigationStore)
        {
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            k = korisnikStore.TrenutniKorisnik;
            Dogadjaji = DogadjajService.sviDogadjajiZaOrganizatora(k.Email);
            _navigationStore = navigationStore;

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
            ObservableCollection<Zadatak> zadaci =ZadatakServce.sviZadaciZaDogadjaj(SelectedDogadjaj.Id);
            TaskViewModel tvm = new TaskViewModel(_navigationStore);
            tvm.Zadaci = zadaci;
            
            tvm.IdDogadjaja = SelectedDogadjaj.Id;
            _navigationStore.CurrentViewModel = tvm;
        }
        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(_detailsCommand => DetailsEvent());
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
            detailsModel.Napomene = SelectedDogadjaj.Napomena;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.Show();

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
            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);


        }


    }
}