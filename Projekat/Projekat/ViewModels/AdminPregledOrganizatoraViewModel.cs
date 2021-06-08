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
using System.Windows.Input;

namespace Projekat.ViewModels
{
    class AdminPregledOrganizatoraViewModel : ViewModelBase
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
                    _deleteCommand = new RelayCommand(_deleteCommand => DeleteEvent());
                return _deleteCommand;
            }
        }

        private void DeleteEvent()
        {
           
            using (var db = new DatabaseContext())
            {
                if (!db.Organizatori.Local.Contains(SelectedOrganizator))
                    db.Organizatori.Attach(SelectedOrganizator);
                db.Dogadjaji.RemoveRange(db.Dogadjaji.Where(d => d.Organizator.Email == SelectedOrganizator.Email));
                
                
                db.Organizatori.Remove(SelectedOrganizator);
                db.SaveChanges();
            }
            Organizatori.Remove(SelectedOrganizator);
            
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
                    _addOrganizatorCommand = new RelayCommand(_addOrganizatorCommand => AddOrganizatorEvent());
                return _addOrganizatorCommand;
            }
        }

        private ICommand _editOrganizatorCommand;
        public ICommand EditOrganizatorCommand
        {
            get
            {
                if (_editOrganizatorCommand == null)
                    _editOrganizatorCommand = new RelayCommand(_editOrganizatorCommand => EditOrganizatorEvent());
                return _editOrganizatorCommand;
            }
        }

        private void EditOrganizatorEvent()
        {
            
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
            AddOrganizator edit = new AddOrganizator(editVM);
            edit.Show();
            Console.WriteLine(editVM.Ulica);

        }

        private void AddOrganizatorEvent()
        {
            AddOrganizator add = new AddOrganizator();
            AddOrganizatorViewModel addVM = new AddOrganizatorViewModel();
            add.Show();
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
    }
}

