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
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ICommand _loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(_loginCommand => LoginUser());
                return _loginCommand;
            }
        }

        private void LoginUser()
        {
            //KorisnikStore...

            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel();
        }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
