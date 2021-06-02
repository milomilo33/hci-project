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

    }
}
