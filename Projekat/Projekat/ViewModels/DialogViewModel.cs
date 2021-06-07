using Projekat.Commands;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class DialogViewModel: ViewModelBase
    {
        public String odgovor;
        private readonly NavigationStore _navigationStore;
        private ICommand _daCommand;
        private ICommand _neCommand;
        public string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand DaCommand
        {
            get
            {
                if (_daCommand == null)
                    _daCommand = new RelayCommand(window => Da((Window)window));
                return _daCommand;
            }
        }
        public ICommand NeCommand
        {
            get
            {
                if (_neCommand == null)
                    _neCommand = new RelayCommand(window => Ne((Window)window));
                return _neCommand;
            }
        }

        public void Da(Window window)
        {
            odgovor = "Da";
            if (window != null)
            {
                window.Close();
            }

        }

        public void Ne(Window window)
        {
            odgovor = "Ne";
            if (window != null)
            {
                window.Close();
            }
        }

        public DialogViewModel()
        {
          
        }

    }
}
