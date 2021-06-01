using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.ViewModels
{
    public class OrganizatorHomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public OrganizatorHomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
