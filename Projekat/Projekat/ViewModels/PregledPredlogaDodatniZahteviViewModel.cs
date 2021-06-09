using Projekat.Commands;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class PregledPredlogaDodatniZahteviViewModel : ViewModelBase 
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewModelBase _previousViewModel;

        public PregledPredlogaDodatniZahteviViewModel(NavigationStore navigationStore, ViewModelBase previousViewModel, ObservableCollection<Predlog> dodatniZahtevi)
        {
            _navigationStore = navigationStore;
            _previousViewModel = previousViewModel;
            _dodatniZahtevi = dodatniZahtevi;
        }

        private ObservableCollection<Predlog> _dodatniZahtevi;
        public ObservableCollection<Predlog> DodatniZahtevi
        {
            get { return _dodatniZahtevi; }
            set
            {
                _dodatniZahtevi = value;
                OnPropertyChanged(nameof(DodatniZahtevi));
            }
        }

        private ICommand _povratakCommand;
        public ICommand PovratakCommand
        {
            get
            {
                if (_povratakCommand == null)
                    _povratakCommand = new RelayCommand(_povratakCommand => Povratak());
                return _povratakCommand;
            }
        }

        public void Povratak()
        {
            _navigationStore.CurrentViewModel = _previousViewModel;
        }

        private ICommand _pocetnaStranicaCommand;

        public ICommand PocetnaStranicaCommand
        {
            get
            {
                if (_pocetnaStranicaCommand == null)
                    _pocetnaStranicaCommand = new RelayCommand(_pocetnaStranicaCommand => PocetnaStrana());
                return _pocetnaStranicaCommand;
            }
        }

        private void PocetnaStrana()
        {
            KorisnikStore korisnik = KorisnikStore.Instance;
            Korisnik k = korisnik.TrenutniKorisnik;

            if (k.GetType() == typeof(Administrator))
            {
                _navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
            }
            else if (k.GetType() == typeof(Organizator))
            {
                _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
            }
            else
            {
                _navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
            }
        }
    }
}
