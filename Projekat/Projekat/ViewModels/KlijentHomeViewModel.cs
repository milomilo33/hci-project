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
                                                    _navigationStore.CurrentViewModel = new KlijentPregledOrganizatoraViewModel(_navigationStore));
                return _pregledOrganizatoraCommand;
            }
        }
    }
}
