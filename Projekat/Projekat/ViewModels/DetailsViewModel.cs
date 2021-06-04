using Projekat.Model;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {

        public DetailsViewModel()
        {
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
        public string _organizator;
        public string Organizator
        {
            get { return _organizator; }
            set
            {
                _organizator = value;
                OnPropertyChanged(nameof(Organizator));
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
        public string _napomene;
        public string Napomene
        {
            get { return _napomene; }
            set
            {
                _napomene = value;
                OnPropertyChanged(nameof(Napomene));
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




    }
}
