using Projekat.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class KlijentKreiranjeDogadjajaDialogViewModel : ViewModelBase
    {
        public KlijentKreiranjeDogadjajaDialogViewModel()
        {

        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private bool _isError;
        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                OnPropertyChanged(nameof(IsError));
            }
        }
        
        private ICommand _zatvoriCommand;

        public ICommand ZatvoriCommand
        {
            get
            {
                if (_zatvoriCommand == null)
                    _zatvoriCommand = new RelayCommand(window => Zatvori((Window) window));
                return _zatvoriCommand;
            }
        }

        public void Zatvori(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
