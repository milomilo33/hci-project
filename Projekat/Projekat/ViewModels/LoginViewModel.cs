﻿using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _lozinka;
        public string Lozinka
        {
            get => _lozinka;
            set
            {
                _lozinka = value;
                OnPropertyChanged(nameof(Lozinka));
            }
        }

        private ICommand _loginCommand;
        private ICommand _registrationCommand;

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(_pass => LoginUser((object) _pass));
                return _loginCommand;
            }
        }

        private void LoginUser(object password)
        {
            var passwordBox = password as PasswordBox;
            Lozinka = passwordBox.Password;
            
            
            using (var db = new DatabaseContext())
            {
                foreach (Korisnik k in db.Korisnici)
                {
                    if (k.Email.Equals(Email))
                    {
                        if (!k.Lozinka.Equals(Lozinka))
                        {
                            SuccessOrErrorDialog dialog1 = new SuccessOrErrorDialog();
                            SuccessOrErrorDialogViewModel dialogModel1 = new SuccessOrErrorDialogViewModel();
                            dialogModel1.IsError = true;
                            dialogModel1.Message = "Nevalidna lozinka!";
                            dialog1.DataContext = dialogModel1;
                            dialog1.ShowDialog();
                            //Console.WriteLine("Nevalidna lozinka");
                            return;
                        }

                        KorisnikStore korisnikStore = KorisnikStore.Instance;
                        korisnikStore.TrenutniKorisnik = k;

                        if (k.GetType() == typeof(Administrator))
                        {
                            //Console.WriteLine("Administrator");
                            _navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
                            return;
                        }
                        else if (k.GetType() == typeof(Klijent))
                        {
                            //Console.WriteLine("Klijent");
                            _navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
                            return;
                        }
                        else if (k.GetType() == typeof(Organizator))
                        {
                            //Console.WriteLine("Organizator");
                            _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
                            return;
                        }
                        else
                        {
                            //Console.WriteLine("Login error");
                            return;
                        }
                    }
                }
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Nepostojeći korisnik!";
                dialog.DataContext = dialogModel;
                dialog.ShowDialog();
                // Console.WriteLine("Nepostojeci korisnik");

            } 
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

            _navigationStore.CurrentViewModel = new RegistrationViewModel(_navigationStore);
        }


        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}


