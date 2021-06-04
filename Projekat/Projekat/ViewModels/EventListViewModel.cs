using Projekat.Model;
using Projekat.Stores;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Projekat.ViewModels
{
    internal class EventListViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        private Dogadjaj _selectedEvent;
        public Dogadjaj SelectedEvent
        {
            get { return _selectedEvent; }
            set { OnPropertyChanged(nameof(Dogadjaj)); }
        }

        public ObservableCollection<Dogadjaj> Events { get; private set; }

        public EventListViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public void LoadEvents(IEnumerable<Dogadjaj> events)
        {
            Events = new ObservableCollection<Dogadjaj>(events);
            OnPropertyChanged(nameof(Dogadjaj));
        }


    }
}