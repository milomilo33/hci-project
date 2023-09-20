using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
    public interface IDogadjajService
    {
        ObservableCollection<Dogadjaj> sviDogadjaji();
        ObservableCollection<Dogadjaj> sviDogadjajiZaOrganizatora(String email);
        ObservableCollection<Dogadjaj> sviDogadjajiZaKlijenta(String email);
        Dogadjaj getDogadjaj(int id);
        void updateStatus(Dogadjaj dogadjaj);
    }
}
