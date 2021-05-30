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
        private ICommand _registrationCommand;

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

        public ICommand RegistrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                    _registrationCommand = new RelayCommand(_registrationCommand => Registration());
                return _registrationCommand;
            }
        }
        private void Registration()
        {

            _navigationStore.CurrentViewModel = new RegistrationViewModel();
        }


        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
