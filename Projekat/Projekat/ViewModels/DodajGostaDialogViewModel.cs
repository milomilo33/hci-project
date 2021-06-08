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
    public class DodajGostaDialogViewModel : ViewModelBase
    {
        public DodajGostaDialogViewModel()
        {
            TrebaDodatiGosta = false;
        }

        public bool TrebaDodatiGosta { get; set; }

        private string _imeIPrezimeGosta;
        public string ImeIPrezimeGosta
        {
            get { return _imeIPrezimeGosta; }
            set
            {
                _imeIPrezimeGosta = value;
                OnPropertyChanged(nameof(ImeIPrezimeGosta));
            }
        }

        private ICommand _dodajCommand;

        public ICommand DodajCommand
        {
            get
            {
                if (_dodajCommand == null)
                    _dodajCommand = new RelayCommand(window => Dodaj((Window)window));
                return _dodajCommand;
            }
        }

        public void Dodaj(Window window)
        {
            TrebaDodatiGosta = true;

            if (window != null)
            {
                window.Close();
            }
        }

        private ICommand _odustaniCommand;

        public ICommand OdustaniCommand
        {
            get
            {
                if (_odustaniCommand == null)
                    _odustaniCommand = new RelayCommand(window => Odustani((Window)window));
                return _odustaniCommand;
            }
        }

        public void Odustani(Window window)
        {
            TrebaDodatiGosta = false;

            if (window != null)
            {
                window.Close();
            }
        }
    }
}
