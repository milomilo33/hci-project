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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    class AdminPregledSaradnikaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ObservableCollection<Saradnik> Saradnici { get; set; }

        private Saradnik _selectedSaradnik;
        public Saradnik SelectedSaradnik
        {
            get => _selectedSaradnik;
            set
            {
                _selectedSaradnik = value;
                OnPropertyChanged(nameof(SelectedSaradnik));
            }
        }

        public AdminPregledSaradnikaViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            using (var db = new DatabaseContext())
            {
                Saradnici = new ObservableCollection<Saradnik>(db.Saradnici.Include("Adresa").Include("Ponude"));
                OnPropertyChanged(nameof(Saradnici));
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

            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!db.Saradnici.Local.Contains(SelectedSaradnik))
                        db.Saradnici.Attach(SelectedSaradnik);
                    //db.Dogadjaji.RemoveRange(db.Dogadjaji.Where(d => d.Sa.Email == SelectedSaradnik.Id));


                    db.Saradnici.Remove(SelectedSaradnik);
                    db.SaveChanges();
                }
                Saradnici.Remove(SelectedSaradnik);

                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste obrisali saradnika!";
                newDialog.DataContext = dialogModel;
                newDialog.Owner = window;
                newDialog.ShowDialog();
            }
            catch (Exception) {
                dialogModel.IsError = true;
                dialogModel.Message = "Došlo je do greške kod brisanja saradnika!";
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

        private ICommand _addSaradnikCommand;
        public ICommand AddSaradnikCommand
        {
            get
            {
                if (_addSaradnikCommand == null)
                    _addSaradnikCommand = new RelayCommand(_addOrganizatorCommand => AddSaradnikEvent());
                    return _addSaradnikCommand;
            }
        }

        private void AddSaradnikEvent()
        {
            AddSaradnik add = new AddSaradnik();
            AddOrganizatorViewModel addVM = new AddOrganizatorViewModel();
            add.Show();
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(_addOrganizatorCommand => EditSaradnikEvent());
                return _editCommand;
            }
        }

        private void EditSaradnikEvent()
        {
            AddSaradnikViewModel editVM = new AddSaradnikViewModel();
            editVM.Naziv = SelectedSaradnik.Naziv;
            editVM.Opis = SelectedSaradnik.Opis;
            editVM.Tip = SelectedSaradnik.Tip;

            if (SelectedSaradnik.Tip == "Lokal")
                editVM.BrojMesta = SelectedSaradnik.BrojMesta.ToString();

            editVM.BrojTelefona = SelectedSaradnik.BrojTelefona;
            editVM.Broj = SelectedSaradnik.Adresa.Broj.ToString();
            editVM.Ulica = SelectedSaradnik.Adresa.Ulica;
            editVM.Grad = SelectedSaradnik.Adresa.Grad;
            editVM.Drzava = SelectedSaradnik.Adresa.Drzava;
            editVM.IsEdit = true;
            editVM.Id = SelectedSaradnik.Id;
            AddSaradnik edit = new AddSaradnik(editVM);
            edit.Show();
            Console.WriteLine(editVM.Ulica);
        }
    }
}
