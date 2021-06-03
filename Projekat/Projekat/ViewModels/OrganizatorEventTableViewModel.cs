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
                if (d.Id == _dogadjaj.Id)
                {
                    d.Status = "Preuzeto";
                    DogadjajService.updateStatus(d);
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
            detailsModel.Napomene = SelectedDogadjaj.Napomena;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.Show();

        }

    }
}
