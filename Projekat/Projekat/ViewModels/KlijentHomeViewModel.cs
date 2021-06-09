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
    public class KlijentHomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public KlijentHomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        private ICommand _pregledOrganizatoraCommand;

        public ICommand PregledOrganizatoraCommand
        {
            get
            {
                if (_pregledOrganizatoraCommand == null)
                    _pregledOrganizatoraCommand = new RelayCommand(_pregledOrganizatoraCommand => 
                                                    _navigationStore.CurrentViewModel = new KlijentPregledOrganizatoraViewModel(_navigationStore, _navigationStore.CurrentViewModel));
                return _pregledOrganizatoraCommand;
            }
        }

        private ICommand _kreiranjeDogadjajaCommand;

        public ICommand KreiranjeDogadjajaCommand
        {
            get
            {
                if (_kreiranjeDogadjajaCommand == null)
                    _kreiranjeDogadjajaCommand = new RelayCommand(_kreiranjeDogadjajaCommand =>
                                                    _navigationStore.CurrentViewModel = new KlijentKreiranjeDogadjajaViewModel(_navigationStore, _navigationStore.CurrentViewModel));
                return _kreiranjeDogadjajaCommand;
            }
        }

        private ICommand _pregledDogadjajaCommand;
        public ICommand PregledDogadjajaCommand
        {
            get
            {
                if (_pregledDogadjajaCommand == null)
                    _pregledDogadjajaCommand = new RelayCommand(_pregledDogadjajaCommand =>
                                                    _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore, _navigationStore.CurrentViewModel ,true));
                return _pregledDogadjajaCommand;
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

            _navigationStore.CurrentViewModel = new OrganizatorProfilViewModel(_navigationStore, _navigationStore.CurrentViewModel);

        }
    }
}
