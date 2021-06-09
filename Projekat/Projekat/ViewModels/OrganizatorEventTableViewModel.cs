using Projekat.Commands;
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
using System.Windows.Input;

namespace Projekat.ViewModels
{
    public class OrganizatorEventTableViewModel : ViewModelBase
    {
        public ObservableCollection<Dogadjaj> dogadjaji;
        private ICommand _acceptCommand;
        private readonly IDogadjajService DogadjajService = new DogadjajService();
        private ICommand _detailsCommand;

        public ObservableCollection<Dogadjaj> Dogadjaji
        {
            get { return dogadjaji; }
            set
            {
                dogadjaji = value;
                OnPropertyChanged(nameof(Dogadjaji));
            }
        }
        public OrganizatorEventTableViewModel()
        {
            Dogadjaji = DogadjajService.sviDogadjaji();

        }
        private Dogadjaj _dogadjaj;
        public Dogadjaj SelectedDogadjaj
        {
            get => _dogadjaj;
            set
            {
                _dogadjaj = value;
                OnPropertyChanged(nameof(SelectedDogadjaj));
            }
        }
        public ICommand AcceptCommand
        {
            get
            {
                if (_acceptCommand == null)
                    _acceptCommand = new RelayCommand(_acceptCommand => Accept());
                return _acceptCommand;
            }
        }
        public void Accept()
        {

            var dd = dogadjaji;
            foreach (var d in dd)
            {
                if (d.Id == _dogadjaj.Id && d.Status.Equals("Nema dodeljenog organizatora"))
                {
                    d.Status = "Čeka se odgovor organizatora";
                    d.StatusEnum = Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR;
                    DogadjajService.updateStatus(d);
                    SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                    dialogModel.IsError = false;
                    dialogModel.Message = "Uspešno prihvaćen događaj!";
                    dialog.DataContext = dialogModel;
                    //dialog.Owner = window;
                    dialog.ShowDialog();
                }
                else if(d.Id == _dogadjaj.Id && !d.Status.Equals("Nema dodeljenog organizatora"))
                {
                    SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                    SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                    dialogModel.IsError = true;
                    dialogModel.Message = "Zadatak je već preuzet!";
                    dialog.DataContext = dialogModel;
                    //dialog.Owner = window;
                    dialog.ShowDialog();

                }
                else
                {
                    break;
                }
            }
            Dogadjaji = null;
            Dogadjaji = dd;


        }
        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(_acceptCommand => DetailsEvent());
                return _detailsCommand;
            }
        }
        public void DetailsEvent()
        {
            Details details = new Details();
            DetailsViewModel detailsModel = new DetailsViewModel();
            detailsModel.Vrsta = SelectedDogadjaj.VrstaProslave;
            detailsModel.Budzet = SelectedDogadjaj.Budzet;
            detailsModel.Tema = SelectedDogadjaj.Tema;
            detailsModel.Organizator = SelectedDogadjaj.Organizator.Ime + " " + SelectedDogadjaj.Organizator.Prezime;
            detailsModel.DatumOdrzavanja = SelectedDogadjaj.DatumOdrzavanja.ToString("dd/MM/yyyy HH:mm");
            detailsModel.DodatniZahtevi = SelectedDogadjaj.DodatniZahtevi;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.Show();

        }

    }
}
