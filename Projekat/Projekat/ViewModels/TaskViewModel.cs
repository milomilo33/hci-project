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
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<Zadatak> _zadaci;
        private readonly IZadatakService ZadatakServce = new ZadatakService();
        private ICommand _addCommand;
        private ICommand _povratakCommand;
        private ICommand _izmenaCommand;
        private ICommand _komentarCommand;
        private ICommand _prihvatiCommand;

        public int _idDogadjaja;
        public NavigationStore _navigationStore;
        public TaskViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
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
        public ObservableCollection<Zadatak> Zadaci
        {
            get { return _zadaci; }
            set
            {
                _zadaci = value;
                OnPropertyChanged(nameof(Zadaci));
            }
        }

        private Zadatak _zadatak;
        public Zadatak SelectedZadatak
        {
            get => _zadatak;
            set
            {
                _zadatak = value;
                OnPropertyChanged(nameof(SelectedZadatak));
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new RelayCommand(_addCommand => Add());
                return _addCommand;
            }
        }
        public void Add()
        {
            CreateTask view = new CreateTask(this);
            KreiranjeZadatkaViewModel viewModel = new KreiranjeZadatkaViewModel();
            viewModel.IdDogadjaja = _idDogadjaja;
            view.DataContext = viewModel;
            view.Show();


        }

        public void refresh()
        {
            Zadaci = ZadatakServce.sviZadaciZaDogadjaj(_idDogadjaja);
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
            _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore);


        }

        public ICommand IzmenaCommand
        {
            get
            {
                if (_izmenaCommand == null)
                    _izmenaCommand = new RelayCommand(window => Izmena((Window)window));
                return _izmenaCommand;
            }
        }
        public ICommand KomentarCommand
        {
            get
            {
                if (_komentarCommand == null)
                    _komentarCommand = new RelayCommand(window => Komentar((Window)window));
                return _komentarCommand;
            }
        }

        public ICommand PrihvatiCommand
        {
            get
            {
                if (_prihvatiCommand == null)
                    _prihvatiCommand = new RelayCommand(window => Prihvati((Window)window));
                return _prihvatiCommand;
            }
        }

        public void Komentar(Window window)
        {
            Komunikacija view = new Komunikacija();
            KomunikacijaViewModel viewModel = new KomunikacijaViewModel();
            viewModel.IdZadatka = SelectedZadatak.Id;
            viewModel.loadKomentari();
            view.DataContext = viewModel;
            view.Owner = window;
            view.Show();
        }
        public void Izmena(Window window)
        {
            IzmenaZadatkaView view = new IzmenaZadatkaView(this);
            IzmenaZadatkaViewModel viewModel = new IzmenaZadatkaViewModel();
            viewModel._navigationStore = _navigationStore;
            viewModel.IdDogadjaja = IdDogadjaja;
            viewModel.zadatak = SelectedZadatak;
            viewModel.Naziv = SelectedZadatak.Naziv;
            viewModel.Opis = SelectedZadatak.Opis;
            view.DataContext = viewModel;
            view.Owner = window;
            view.Show();

        }

        public void Prihvati(Window window)
        {
            PredloziZaZadatak view = new PredloziZaZadatak();
            PredloziZaZadatakViewModel viewModel = new PredloziZaZadatakViewModel();
           // viewModel.Ponude = 
           // view.DataContext = viewModel;
            view.Owner = window;
            view.Show();

        }

    }
}
