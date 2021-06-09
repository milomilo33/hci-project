using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
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
using static Projekat.ViewModels.KreiranjeZadatkaViewModel;

namespace Projekat.ViewModels
{
    public class KreiranjeZadatkaViewModel:ViewModelBase, ICloseWindow
    {

        private readonly NavigationStore _navigationStore;
        private ICommand _submitCommand;
        private IZadatakService ZadatakService = new ZadatakService();
        public int _idDogadjaja;
        public Dogadjaj dogadjaj;

        public int IdDogadjaja
        {
            get => _idDogadjaja;
            set
            {
                _idDogadjaja = value;
                OnPropertyChanged(nameof(IdDogadjaja));
            }
        }

        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set
            {
                _naziv = value;
                OnPropertyChanged(nameof(Naziv));
            }
        }
        private string _izabranTip;
        public string IzabranTip
        {
            get => _izabranTip;
            set
            {
                _izabranTip = value;
                OnPropertyChanged(nameof(IzabranTip));
            }
        }
        private string _opis;
        public string Opis
        {
            get => _opis;
            set
            {
                _opis = value;
                OnPropertyChanged(nameof(Opis));
            }
        }
        

        private ObservableCollection<String> _tipovi;
        public ObservableCollection<String> Tipovi
        {
            get { return _tipovi; }
            set
            {
                _tipovi = value;
                OnPropertyChanged(nameof(Tipovi));
            }
        }
        public ICommand AddCommand
        {
            get
            {
                if (_submitCommand == null)
                    _submitCommand = new RelayCommand(_submitCommand => Add());
                return _submitCommand;
            }
        }

        public Action Close { get ; set; }

        private void Add()
        {
            
            using (var db = new DatabaseContext())
            {
                Zadatak zadatak = new Zadatak();
                zadatak.Naziv = Naziv;
                zadatak.Opis = Opis;
                //zadatak.Status = "Na čekanju";
                if(IzabranTip.Equals("Glavni zahtev"))
                {
                    zadatak.Tip = Zadatak.TipZadatka.GLAVNI;
                }
                else
                {
                    zadatak.Tip = Zadatak.TipZadatka.DODATNI;
                }
                
                dogadjaj = db.Dogadjaji.SingleOrDefault(d => d.Id == IdDogadjaja);
                zadatak.Dogadjaj = dogadjaj;
                db.Zadaci.Add(zadatak);
                db.SaveChanges();
            }
            SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
            SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
            dialogModel.IsError = false;
            dialogModel.Message = "Uspešno kreiran zadatak!";
            dialog.DataContext = dialogModel;
            //dialog.Owner = window;
            dialog.ShowDialog();
            CloseWindow();
         



        }
        public KreiranjeZadatkaViewModel()
        {
        }

        public KreiranjeZadatkaViewModel(int id)
        {
            IdDogadjaja = id;
            ObservableCollection<string> list = new ObservableCollection<string>();
            if (ZadatakService.proveraDaLiPostoji(IdDogadjaja))
            {
                list.Add("Glavni zahtev");
                list.Add("Dodatni zahtev");
            }
            else
            {
                list.Add("Dodatni zahtev");
            }

            Tipovi = list;
        }

        private void CloseWindow()
        {
            Close?.Invoke();
        }

        public interface ICloseWindow
        {
            Action Close { get; set; }
        }
    }
}
