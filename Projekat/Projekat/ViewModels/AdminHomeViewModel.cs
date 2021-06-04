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
    public class AdminHomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public AdminHomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        private ICommand _eventListViewCommand;
        public ICommand EventListViewCommand
        {
            get
            {
                Console.WriteLine("AAAAAAA");
                if (_eventListViewCommand == null)
                    _eventListViewCommand = new RelayCommand(_eventListView => ViewEvents());
                return _eventListViewCommand;
            }

        }

        private void ViewEvents() 
        {
            _navigationStore.CurrentViewModel = new EventListViewModel(_navigationStore);
        }
    }
}
