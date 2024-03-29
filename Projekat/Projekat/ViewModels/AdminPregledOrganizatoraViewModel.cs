﻿using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AdminPregledOrganizatoraViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ObservableCollection<Organizator> Organizatori { get; set; }

        private Organizator _selectedOrganizator;
        public Organizator SelectedOrganizator
        {
            get => _selectedOrganizator;
            set
             {
                _selectedOrganizator = value;
                OnPropertyChanged(nameof(SelectedOrganizator));
            }
        }
        public AdminPregledOrganizatoraViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            // ucitavanje organizatora
            using (var db = new DatabaseContext())
            {
                //foreach (Adresa a in db.Adrese)
                //{
                //    Console.WriteLine(a.Grad);
                //}
                Organizatori = new ObservableCollection<Organizator>(db.Organizatori.Include("Adresa"));
                OnPropertyChanged(nameof(Organizatori));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(window => DeleteEvent((Window)window));
                return _deleteCommand;
            }
        }

        private void DeleteEvent(Window window)
        {

            SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();

            if (SelectedOrganizator == null)
            {
                dialogModel.IsError = true;
                dialogModel.Message = "Niste odabrali organizatora za brisanje.";
                newDialog.DataContext = dialogModel;
                newDialog.Owner = window;
                newDialog.ShowDialog();
                return;
            }

            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!db.Organizatori.Local.Contains(SelectedOrganizator))
                        db.Organizatori.Attach(SelectedOrganizator);
                    db.Dogadjaji.RemoveRange(db.Dogadjaji.Where(d => d.Organizator.Email == SelectedOrganizator.Email));

                    Dialog dialog = new Dialog();
                    DialogViewModel viewModel = new DialogViewModel();
                    viewModel._message = "Da li ste sigurni da želite da obrišete organizatora?";
                    dialog.DataContext = viewModel;
                    dialog.Owner = window;
                    dialog.ShowDialog();

                    if (viewModel.odgovor == "Da")
                    {
                        db.Organizatori.Remove(SelectedOrganizator);
                        db.SaveChanges();
                        Organizatori.Remove(SelectedOrganizator);

                    }

                }
            }
            catch (Exception)
            {
                dialogModel.IsError = true;
                dialogModel.Message = "Došlo je do greške kod brisanja organizatora!";
                newDialog.DataContext = dialogModel;
                newDialog.Owner = window;
                newDialog.ShowDialog();
            }


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
            _navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
        }

        private ICommand _addOrganizatorCommand;
        public ICommand AddOrganizatorCommand
        {
            get
            {
                if (_addOrganizatorCommand == null)
                    _addOrganizatorCommand = new RelayCommand(window => AddOrganizatorEvent((Window) window));
                return _addOrganizatorCommand;
            }
        }

        private ICommand _editOrganizatorCommand;
        public ICommand EditOrganizatorCommand
        {
            get
            {
                if (_editOrganizatorCommand == null)
                    _editOrganizatorCommand = new RelayCommand(window => EditOrganizatorEvent((Window) window));
                return _editOrganizatorCommand;
            }
        }

        private void EditOrganizatorEvent(Window window)
        {
            
            if(SelectedOrganizator == null)
            {
                SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Niste odabrali organizatora za izmenu.";
                newDialog.DataContext = dialogModel;
                newDialog.Owner = window;
                newDialog.ShowDialog();
                return;
            }

            AddOrganizatorViewModel editVM = new AddOrganizatorViewModel();
            editVM.Email = SelectedOrganizator.Email;
            editVM.Ime = SelectedOrganizator.Ime;
            editVM.Prezime = SelectedOrganizator.Prezime;
            editVM.Lozinka = SelectedOrganizator.Lozinka;
            editVM.PonovljenaLozinka = SelectedOrganizator.Lozinka;
            editVM.BrojTelefona = SelectedOrganizator.BrojTelefona;
            editVM.Broj = SelectedOrganizator.Adresa.Broj.ToString();
            editVM.Ulica = SelectedOrganizator.Adresa.Ulica;
            editVM.Grad = SelectedOrganizator.Adresa.Grad;
            editVM.Drzava = SelectedOrganizator.Adresa.Drzava;
            editVM.IsEdit = true;
            AddOrganizator edit = new AddOrganizator(this);
            edit.DataContext = editVM;
            edit.Owner = window;
            edit.ShowDialog();
            Console.WriteLine(editVM.Ulica);

        }

        private void AddOrganizatorEvent(Window window)
        {
            AddOrganizatorViewModel addVM = new AddOrganizatorViewModel(); 
            AddOrganizator add = new AddOrganizator(this);
            add.DataContext = addVM;
            add.Owner = window;
            add.ShowDialog();
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
        public void refresh()
        {
            using (var db = new DatabaseContext())
            {
                Organizatori = new ObservableCollection<Organizator>(db.Organizatori.Include("Adresa"));
                OnPropertyChanged(nameof(Organizatori));
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

