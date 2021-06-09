using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AdminPregledSaradnikaViewModel : ViewModelBase
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

        public void refresh()
        {
            using (var db = new DatabaseContext())
            {
                Saradnici = new ObservableCollection<Saradnik>(db.Saradnici.Include("Adresa"));
                OnPropertyChanged(nameof(Saradnici));
            }
        }

        private void DeleteEvent(Window window)
        {
            SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();

            if (SelectedSaradnik == null)
            {
                dialogModel.IsError = true;
                dialogModel.Message = "Niste odabrali saradnika za brisanje.";
                newDialog.DataContext = dialogModel;
                newDialog.ShowDialog();
                return;
            }

            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!db.Saradnici.Local.Contains(SelectedSaradnik))
                        db.Saradnici.Attach(SelectedSaradnik);
                    //db.Dogadjaji.RemoveRange(db.Dogadjaji.Where(d => d.Sa.Email == SelectedSaradnik.Id));
                    Dialog dialog = new Dialog();
                    DialogViewModel viewModel = new DialogViewModel();
                    viewModel._message = "Da li ste sigurni da želite da izbrišete saradnika?";
                    dialog.DataContext = viewModel;
                    dialog.ShowDialog();

                    if (viewModel.odgovor == "Da")
                    {
                        db.Saradnici.Remove(SelectedSaradnik);
                        db.SaveChanges();
                        Saradnici.Remove(SelectedSaradnik);

                    }
                }
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
            AddSaradnikViewModel addVM = new AddSaradnikViewModel();
            AddSaradnik add = new AddSaradnik(this);
            add.DataContext = addVM;
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
            

            if (SelectedSaradnik == null)
            {
                SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Niste odabrali saradnika za izmenu.";
                newDialog.DataContext = dialogModel;
                newDialog.ShowDialog();
                return;
            }

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
            AddSaradnik edit = new AddSaradnik(this);
            edit.DataContext = editVM;
            edit.Show();
            Console.WriteLine(editVM.Ulica);
        }
    }
}
