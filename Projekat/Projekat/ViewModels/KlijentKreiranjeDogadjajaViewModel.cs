using Projekat.Commands;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class KlijentKreiranjeDogadjajaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public KlijentKreiranjeDogadjajaViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public string _vrsta;
        public string Vrsta
        {
            get { return _vrsta; }
            set
            {
                _vrsta = value;
                OnPropertyChanged(nameof(Vrsta));
            }
        }

        public string _tema;
        public string Tema
        {
            get { return _tema; }
            set
            {
                _tema = value;
                OnPropertyChanged(nameof(Tema));
            }
        }

        public Organizator _organizator;
        public Organizator Organizator
        {
            get { return _organizator; }
            set
            {
                _organizator = value;
                OnPropertyChanged(nameof(Organizator));
            }
        }

        public string _imeOrganizatora;
        public string ImeOrganizatora
        {
            get { return _imeOrganizatora; }
            set
            {
                _imeOrganizatora = value;
                OnPropertyChanged(nameof(ImeOrganizatora));
            }
        }

        public double _budzet;
        public double Budzet
        {
            get { return _budzet; }
            set
            {
                _budzet = value;
                OnPropertyChanged(nameof(Budzet));
            }
        }

        public bool _budzetPromenljiv;
        public bool BudzetPromenljiv
        {
            get { return _budzetPromenljiv; }
            set
            {
                _budzetPromenljiv = value;
                OnPropertyChanged(nameof(BudzetPromenljiv));
            }
        }

        public string _datum;
        public string DatumOdrzavanja
        {
            get { return _datum; }
            set
            {
                _datum = value;
                OnPropertyChanged(nameof(DatumOdrzavanja));
            }
        }

        public double _broj;
        public double BrojGostiju
        {
            get { return _broj; }
            set
            {
                _broj = value;
                OnPropertyChanged(nameof(BrojGostiju));
            }
        }

        public string _dodatniZahtevi;
        public string DodatniZahtevi
        {
            get { return _dodatniZahtevi; }
            set
            {
                _dodatniZahtevi = value;
                OnPropertyChanged(nameof(DodatniZahtevi));
            }
        }

        public string _mesto;
        public string MestoOdrzavanja
        {
            get { return _mesto; }
            set
            {
                _mesto = value;
                OnPropertyChanged(nameof(MestoOdrzavanja));
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
            _navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
        }
    }
}
