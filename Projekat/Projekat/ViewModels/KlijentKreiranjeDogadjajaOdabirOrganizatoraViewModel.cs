using Projekat.Commands;
using Projekat.Data;
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
    public class KlijentKreiranjeDogadjajaOdabirOrganizatoraViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private readonly ViewModelBase _previousViewModel;

        public ObservableCollection<Organizator> Organizatori { get; set; }

        public KlijentKreiranjeDogadjajaOdabirOrganizatoraViewModel(NavigationStore navigationStore, ViewModelBase previousViewModel)
        {
            _navigationStore = navigationStore;
            _previousViewModel = previousViewModel;

            // ucitavanje organizatora
            using (var db = new DatabaseContext())
            {
                Organizatori = new ObservableCollection<Organizator>(db.Organizatori.Include("Adresa"));
                OnPropertyChanged(nameof(Organizatori));
            }
        }

        private ICommand _odaberiCommand;

        public ICommand OdaberiCommand
        {
            get
            {
                if (_odaberiCommand == null)
                    _odaberiCommand = new RelayCommand(email => Odaberi((string) email));
                return _odaberiCommand;
            }
        }

        public void Odaberi(string organizatorEmail)
        {
            Organizator organizator;
            using (var db = new DatabaseContext())
            {
                organizator = db.Organizatori.SingleOrDefault(o => o.Email.Equals(organizatorEmail));
            }
            ((KlijentKreiranjeDogadjajaViewModel)_previousViewModel).Organizator = organizator;
            ((KlijentKreiranjeDogadjajaViewModel)_previousViewModel).ImeOrganizatora = organizator.Ime + " " + organizator.Prezime;
            _navigationStore.CurrentViewModel = _previousViewModel;
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
    }
}
