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
using static Projekat.ViewModels.KreiranjeZadatkaViewModel;

namespace Projekat.ViewModels
{
    public class IzmenaZadatkaViewModel : ViewModelBase, ICloseWindow


    {
        public NavigationStore _navigationStore;
        private ICommand _izmeniCommand;
        private ICommand _odustaniCommand;
        public Zadatak zadatak;
        public int _idDogadjaja;
        public ObservableCollection<Zadatak> _zadaci;
        private readonly IZadatakService ZadatakServoce = new ZadatakService();

        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }
        private string _opis;
        public string Opis
        {
            get => _opis;
            set
            {
                _opis = value;
                OnPropertyChanged(nameof(Opis));
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

        public ICommand IzmeniCommand
        {
            get
            {
                if (_izmeniCommand == null)
                    _izmeniCommand = new RelayCommand(window => Edit((Window)window));
                return _izmeniCommand;
            }
        }
        public ICommand OdustaniCommand
        {
            get
            {
                if (_odustaniCommand == null)
                    _odustaniCommand = new RelayCommand(window => Cancel((Window)window));
                return _odustaniCommand;
            }
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

        private void Edit(Window window)
        {
            KlijentKreiranjeDogadjajaDialog dialog = new KlijentKreiranjeDogadjajaDialog();
            KlijentKreiranjeDogadjajaDialogViewModel dialogModel = new KlijentKreiranjeDogadjajaDialogViewModel();
            if (zadatak.Opis == Opis && zadatak.Naziv == Naziv)
            {
                dialogModel.IsError = true;
                dialogModel.Message = "Ništa nije izmenjeno!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

            }
            else
            {
                using (var db = new DatabaseContext())
                {
                    zadatak = db.Zadaci.SingleOrDefault(z => z.Id == zadatak.Id);
                    zadatak.Opis = Opis;
                    zadatak.Naziv = Naziv;
                    db.SaveChanges();
                }

                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno ste izmenili zadatak!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
                CloseWindow();
                //Povratak();
            }


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
                CloseWindow();
            }

        }

        public void Povratak()
        {

            using (var db = new DatabaseContext())
            {
                Zadaci = new ObservableCollection<Zadatak>(db.Zadaci.Include("Dogadjaj").Where(d => d.Dogadjaj.Id.Equals(IdDogadjaja)));
            }
            TaskViewModel tvm = new TaskViewModel(_navigationStore);
            tvm.Zadaci = Zadaci;
            tvm.IdDogadjaja = IdDogadjaja;
            _navigationStore.CurrentViewModel = tvm;


        }


        public IzmenaZadatkaViewModel()
        {

        }
        public Action Close { get; set; }
        private void CloseWindow()
        {
            Close?.Invoke();
        }

       
    }
}
