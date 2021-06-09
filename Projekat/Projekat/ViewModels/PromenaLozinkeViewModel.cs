using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class PromenaLozinkeViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private ICommand _editCommand;
        private ICommand _cancelCommand;
        private ICommand _povratakCommand;
        
        public Korisnik korisnik;

        private string _lozinka;
        public string Lozinka
        {
            get => _lozinka;
            set
            {
                _lozinka = value;
                OnPropertyChanged(nameof(Lozinka));
            }
        }
        private string _ponovljenalozinka;
        public string PonovljenaLozinka
        {
            get => _ponovljenalozinka;
            set
            {
                _ponovljenalozinka = value;
                OnPropertyChanged(nameof(PonovljenaLozinka));
            }
        }

        private string _staraLozinka;
        public string StaraLozinka
        {
            get => _staraLozinka;
            set
            {
                _staraLozinka = value;
                OnPropertyChanged(nameof(StaraLozinka));
            }
        }

        public PromenaLozinkeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
           
            
        }


        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(window => Edit((Window)window));
                return _editCommand;
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(window => Cancel((Window)window));
                return _cancelCommand;
            }
        }

        private void Edit(Window window)
        {
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            korisnik = LoadKorisnik(korisnikStore.TrenutniKorisnik.Email);
            string validationMessage = ValidationMessage();
            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                dialogModel.IsError = true;
                dialogModel.Message = validationMessage;
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                using (var db = new DatabaseContext())
                {
                    korisnik = db.Korisnici.Include("Adresa").SingleOrDefault(o => o.Email == korisnik.Email);
                    korisnik.Lozinka = Lozinka;
                    db.SaveChanges();
                }

                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste izmenili lozinku!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
                Povratak();
            }


        }
        private string ValidationMessage()
        {
            string message = "";

            if (string.IsNullOrWhiteSpace(StaraLozinka))
            {
                message += "Morate navesti staru lozinku!\n\n";
            }
            if (string.IsNullOrWhiteSpace(Lozinka))
            {
                message += "Morate navesti novu lozinku!\n\n";
            }
            if (string.IsNullOrWhiteSpace(PonovljenaLozinka))
            {
                message += "Morate navesti ponovljenu novu lozinku!\n\n";
            }
            
            if (!korisnik.Lozinka.Equals(StaraLozinka))
            {
                message += "Navedena stara lozinka nije ispravna!\n\n";
            }
           
            if (!PonovljenaLozinka.Equals(Lozinka))
            {
                message += "Lozinke se moraju poklapati!\n\n";
            }
           


            return message;
        }
        private void Cancel(Window window)
        {
            Dialog dialog = new Dialog();
            DialogViewModel viewModel = new DialogViewModel();
            viewModel._message = "Da li želite da odustanete?";
            dialog.DataContext = viewModel;
            dialog.Owner = window;
            dialog.ShowDialog();

            if (viewModel.odgovor == "Da")
            {
                Povratak();
            }

        }
        private Korisnik LoadKorisnik(String email)
        {
            Korisnik korisnik;
            using (var db = new DatabaseContext())
            {
                korisnik = db.Korisnici.Include("Adresa").SingleOrDefault(o => o.Email == email);
            }
            return korisnik;

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
