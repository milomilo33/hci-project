using Projekat.Commands;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
   public class PredloziZaZadatakViewModel: ViewModelBase
    {
        private ICommand _acceptCommand;
        private Ponuda _ponuda;
        public ObservableCollection<Ponuda> _ponude;

        public PredloziZaZadatakViewModel()
        {

        }


        public ObservableCollection<Ponuda> Ponude
        {
            get { return _ponude; }
            set
            {
                _ponude = value;
                OnPropertyChanged(nameof(Ponude));
            }
        }

        public Ponuda SelectedPonuda
        {
            get => _ponuda;
            set
            {
                _ponuda = value;
                OnPropertyChanged(nameof(SelectedPonuda));
            }
        }

        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(_acceptCommand => Add());
                return _acceptCommand;
            }
        }
        public void Add()
        {
            

        }

    }
}
