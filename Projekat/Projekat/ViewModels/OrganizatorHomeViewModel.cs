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
    public class OrganizatorHomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public OrganizatorHomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        private ICommand _pregledPonudaCommand;

        public ICommand PregledPonudaCommand 
        {
            get
            {
                if (_pregledPonudaCommand == null)
                    _pregledPonudaCommand = new RelayCommand(_pregledPonudaCommand => PregledajPonude());
                return _pregledPonudaCommand;
            }
        }

        private void PregledajPonude()
        {

            _navigationStore.CurrentViewModel = new PregledPonudaViewModel(_navigationStore);
        }

        


       private ICommand _pregledDogadjajaCommand;

        public ICommand PregledDogadjajaCommand
        {
            get
            {
                if (_pregledDogadjajaCommand == null)
                    _pregledDogadjajaCommand = new RelayCommand(_pregledPonudaCommand => PregledajDogadjaje());
                return _pregledDogadjajaCommand;
            }
        }

        private void PregledajDogadjaje()
        {

            _navigationStore.CurrentViewModel = new OrganizatorEventTableViewModel();
        }

        private ICommand _pregledSaradnikaCommand;

        public ICommand PregledSaradnikaCommand
        {
            get
            {
                if (_pregledSaradnikaCommand == null)
                    _pregledSaradnikaCommand = new RelayCommand(_pregledPonudaCommand => PregledajSaradnike());
                return _pregledSaradnikaCommand;
            }
        }

        private void PregledajSaradnike()
        {

            _navigationStore.CurrentViewModel = new AdminPregledSaradnikaViewModel(_navigationStore);
        }


        private ICommand _izabraniDogadjajiCommand;

        public ICommand IzabraniDogadjajiCommand
        {
            get
            {
                if (_izabraniDogadjajiCommand == null)
                    _izabraniDogadjajiCommand = new RelayCommand(_izabraniDogadjajiCommand => IzabraniDogadjaji());
                return _izabraniDogadjajiCommand;
            }
        }

        private void IzabraniDogadjaji()
        {

            _navigationStore.CurrentViewModel = new OrganizatorDodeljeniDogadjajiViewModel(_navigationStore);

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
