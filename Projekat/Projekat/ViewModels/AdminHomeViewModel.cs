using Projekat.Commands;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class AdminHomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public AdminHomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        private ICommand _eventListViewCommand;
        public ICommand EventListViewCommand
        {
            get
            {
                if (_eventListViewCommand == null)
                    _eventListViewCommand = new RelayCommand(_eventListView => ViewEvents());
                return _eventListViewCommand;
            }

        }

        private void ViewEvents() 
        {
            _navigationStore.CurrentViewModel = new EventListViewModel(_navigationStore);
        }

        private ICommand _pregledOrganizatoraCommand;
        public ICommand PregledOrganizatoraCommand
        {
            get
            {
                if (_pregledOrganizatoraCommand == null)
                    _pregledOrganizatoraCommand = new RelayCommand(_pregledOrganizatoraCommand =>
                                                    _navigationStore.CurrentViewModel = new AdminPregledOrganizatoraViewModel(_navigationStore));
                return _pregledOrganizatoraCommand;
            }
        }

        private ICommand _pregledSaradnikaCommand;

        public ICommand PregledSaradnikaCommand
        {
            get
            {
                if (_pregledSaradnikaCommand == null)
                    _pregledSaradnikaCommand = new RelayCommand(_pregledSaradnikaCommand =>
                                                       _navigationStore.CurrentViewModel = new AdminPregledSaradnikaViewModel(_navigationStore));
                return _pregledSaradnikaCommand;
            }
        }

        private ICommand _pregledKlijenataCommand;
        public ICommand PregledKlijenataCommand
        {
            get
            {
                if (_pregledKlijenataCommand == null)
                    _pregledKlijenataCommand = new RelayCommand(_pregledKlijenataCommand =>
                                                    _navigationStore.CurrentViewModel = new AdminPregledKlijenataViewModel(_navigationStore));
                return _pregledKlijenataCommand;
            }
        }
        private ICommand _profilCommand;
        public ICommand ProfilCommand
        {
            get
            {
                if (_profilCommand == null)
                    _profilCommand = new RelayCommand(_profilCommand => Profil());
                return _profilCommand;
            }
        }

        private void Profil()
        {

            _navigationStore.CurrentViewModel = new OrganizatorProfilViewModel(_navigationStore);

        }
    }
}
