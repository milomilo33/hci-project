using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
    public interface IZadatakService
    {
        
        ObservableCollection<Zadatak> sviZadaciZaDogadjaj(int id);

        Zadatak getZadatak(int id);
        Zadatak getZadatakSaPredlogom(int id);

        Boolean proveraGlavniZadatak(int id);

        Boolean proveraDaLiPostoji(int id);
      
    }
}
