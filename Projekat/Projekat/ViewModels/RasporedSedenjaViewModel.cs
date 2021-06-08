using Projekat.Commands;
using Projekat.Model;
using Projekat.Stores;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class RasporedSedenjaViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ViewModelBase _previousViewModel;

        private Dogadjaj _dogadjaj;

        public RasporedSedenjaViewModel(NavigationStore navigationStore, ViewModelBase previousViewModel, Dogadjaj dogadjaj)
        {
            _navigationStore = navigationStore;
            _previousViewModel = previousViewModel;
            _dogadjaj = dogadjaj;

            if (dogadjaj.NerasporedjeniGosti == null)
            {
                dogadjaj.NerasporedjeniGosti = new List<Gost>();
            }

            NerasporedjeniGosti = new ObservableCollection<Gost>(dogadjaj.NerasporedjeniGosti);


            RasporedSedenja = new ObservableCollection<KapacitetStola>(dogadjaj.RasporedSedenja);
        }

        private ObservableCollection<Gost> _nerasporedjeniGosti;
        public ObservableCollection<Gost> NerasporedjeniGosti
        {
            get { return _nerasporedjeniGosti; }
            set
            {
                _nerasporedjeniGosti = value;
                OnPropertyChanged(nameof(NerasporedjeniGosti));
            }
        }

        private ObservableCollection<KapacitetStola> _rasporedSedenja;
        public ObservableCollection<KapacitetStola> RasporedSedenja
        {
            get { return _rasporedSedenja; }
            set
            {
                _rasporedSedenja = value;
                OnPropertyChanged(nameof(RasporedSedenja));
            }
        }

        public void PropertiesChanged()
        {
            //OnPropertyChanged(nameof(RasporedSedenja));
            //OnPropertyChanged(nameof(NerasporedjeniGosti));
        }

        private ICommand _dodajNerasporedjenogGostaCommand;
        public ICommand DodajNerasporedjenogGostaCommand
        {
            get
            {
                if (_dodajNerasporedjenogGostaCommand == null)
                    _dodajNerasporedjenogGostaCommand = new RelayCommand(window => DodajNerasporedjenogGosta((Window) window));
                return _dodajNerasporedjenogGostaCommand;
            }
        }

        private void DodajNerasporedjenogGosta(Window window)
        {
            if (NerasporedjeniGosti.Count() >= _dogadjaj.BrojGostiju)
            {
                Console.WriteLine("dijalog sa greskom broja gostiju");
            }
            else
            {
                DodajGostaDialogView dialog = new DodajGostaDialogView();
                DodajGostaDialogViewModel dialogModel = new DodajGostaDialogViewModel();
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();

                if (dialogModel.TrebaDodatiGosta)
                {
                    Gost noviGost = new Gost();
                    // dodati proveru i za rasporedjene goste
                    foreach (Gost g in NerasporedjeniGosti)
                    {
                        if (g.ImeIPrezime.Equals(dialogModel.ImeIPrezimeGosta))
                        {
                            Console.WriteLine("dijalog sa greskom postojece ime");
                            return;
                        }
                    }
                    noviGost.ImeIPrezime = dialogModel.ImeIPrezimeGosta;
                    NerasporedjeniGosti.Add(noviGost);
                }
            }
        }

        private ICommand _obrisiNerasporedjenogGostaCommand;
        public ICommand ObrisiNerasporedjenogGostaCommand
        {
            get
            {
                if (_obrisiNerasporedjenogGostaCommand == null)
                    _obrisiNerasporedjenogGostaCommand = new RelayCommand(imeIPrezimeGosta => ObrisiNerasporedjenogGosta((string) imeIPrezimeGosta));
                return _obrisiNerasporedjenogGostaCommand;
            }
        }

        private void ObrisiNerasporedjenogGosta(string imeIPrezimeGosta)
        {
            Gost gost = NerasporedjeniGosti.SingleOrDefault(g => g.ImeIPrezime.Equals(imeIPrezimeGosta));
            NerasporedjeniGosti.Remove(gost);

            //dijalog?
        }
    }
}
