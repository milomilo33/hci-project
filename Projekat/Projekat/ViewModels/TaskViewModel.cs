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
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class TaskViewModel:ViewModelBase
    {
        public ObservableCollection<Zadatak> _zadaci;
        private readonly IZadatakService ZadatakServce = new ZadatakService();
        private ICommand _addCommand;
        private ICommand _povratakCommand;
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

    }
}
