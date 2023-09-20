using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Projekat.ViewModels.IzmjenaPonudeViewModel;

namespace Projekat.ViewModels
{
   public class PredloziZaZadatakViewModel: ViewModelBase, ICloseWindow
    {
        private ICommand _acceptCommand;
        private Ponuda _ponuda;
        public ObservableCollection<Ponuda> _ponude;
        private readonly IZadatakService ZadatakService = new ZadatakService();
        private readonly IPonudaService PonudaService = new PonudaService();
        private int _idZadatka;
      

        public PredloziZaZadatakViewModel()
        {

        }
        public int IdZadatka
        {
            get => _idZadatka;
            set
            {
                _idZadatka = value;
                OnPropertyChanged(nameof(IdZadatka));
            }
        }

       


        public ObservableCollection<Ponuda> Ponude
        {
            get { return _ponude; }
            set
            {
                _ponude = value;
                OnPropertyChanged(nameof(Ponude));
            }
        }

        public Ponuda SelectedPonuda
        {
            get => _ponuda;
            set
            {
                _ponuda = value;
                OnPropertyChanged(nameof(SelectedPonuda));
            }
        }

        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(window => Add((Window)window));
                return _acceptCommand;
            }
        }

       
        
        public void Add(Window window)
        {

            if (SelectedPonuda == null)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Morate selektovati ponudu da biste je prihvatili!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
                CloseWindow();

            }
            else
            {
                Predlog p = new Predlog();
                p.Status = Predlog.STATUS.NA_CEKANJU;

                using (var db = new DatabaseContext())
                {
                    var zadatak = db.Zadaci.Include("Dogadjaj").SingleOrDefault(d => d.Id == IdZadatka);
                    var ponuda = db.Ponude.Find(SelectedPonuda.Id);
                    zadatak.IzabraniPredlog = p;
                    p.Zadatak = zadatak;
                    p.Ponuda = ponuda;

                    if (zadatak.Tip == Zadatak.TipZadatka.GLAVNI)
                    {
                        Dogadjaj dog = db.Dogadjaji.Include("NerasporedjeniGosti")
                                                   .Include("RasporedSedenja")
                                                   .SingleOrDefault(d => d.Id == zadatak.Dogadjaj.Id);
                        dog.NerasporedjeniGosti = null;
                        dog.RasporedSedenja = null;
                    }

                    db.Predlozi.Add(p);
                    db.SaveChanges();
                }

                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = false;
                dialogModel.Message = "Uspešno prihvaćena ponuda!";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
                CloseWindow();
            }
        }
        public Action Close { get; set; }
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
