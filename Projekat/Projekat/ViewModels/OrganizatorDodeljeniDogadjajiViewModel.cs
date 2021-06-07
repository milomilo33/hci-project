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

namespace Projekat.ViewModels
{
    public class OrganizatorDodeljeniDogadjajiViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ObservableCollection<Dogadjaj> dogadjaji;
        private ICommand _taskCommand;
        private readonly IDogadjajService DogadjajService = new DogadjajService();
        private readonly IZadatakService ZadatakServce = new ZadatakService();
        private ICommand _detailsCommand;
        private ICommand _povratakCommand;
        private Korisnik k;

        public ObservableCollection<Dogadjaj> Dogadjaji
        {
            get { return dogadjaji; }
            set
            {
                dogadjaji = value;
                OnPropertyChanged(nameof(Dogadjaji));
            }
        }
        public OrganizatorDodeljeniDogadjajiViewModel(NavigationStore navigationStore, bool isKlijent = false)
        {
            KorisnikStore korisnikStore = KorisnikStore.Instance;
            k = korisnikStore.TrenutniKorisnik;

            if (isKlijent)
            {
                Dogadjaji = DogadjajService.sviDogadjajiZaKlijenta(k.Email);
            }
            else
            {
                Dogadjaji = DogadjajService.sviDogadjajiZaOrganizatora(k.Email);
            }

            _navigationStore = navigationStore;
            IsKlijent = isKlijent;
        }

        private bool _isKlijent;
        public bool IsKlijent
        {
            get { return _isKlijent; }
            set
            {
                _isKlijent = value;
                OnPropertyChanged(nameof(IsKlijent));
            }
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
        public ICommand TaskCommand
        {
            get
            {
                if (_taskCommand == null)
                    _taskCommand = new RelayCommand(_taskCommand => Tasks());
                return _taskCommand;
            }
        }
        public void Tasks()
        {
            ObservableCollection<Zadatak> zadaci =ZadatakServce.sviZadaciZaDogadjaj(SelectedDogadjaj.Id);
            TaskViewModel tvm = new TaskViewModel(_navigationStore);
            tvm.Zadaci = zadaci;
            
            tvm.IdDogadjaja = SelectedDogadjaj.Id;
            _navigationStore.CurrentViewModel = tvm;
        }

        private ICommand _predlogOrganizatoraCommand;
        public ICommand PredlogOrganizatoraCommand
        {
            get
            {
                if (_predlogOrganizatoraCommand == null)
                    _predlogOrganizatoraCommand = new RelayCommand(window => OtvoriPredlogOrganizatora((Window) window));
                return _predlogOrganizatoraCommand;
            }
        }
        public void OtvoriPredlogOrganizatora(Window window)
        {
            if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Čeka se predlog organizatora.";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else if (SelectedDogadjaj.StatusEnum == Dogadjaj.STATUS_DOGADJAJA.NEDODELJEN)
            {
                SuccessOrErrorDialog dialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Nijedan organizator nije još prihvatio događaj.";
                dialog.DataContext = dialogModel;
                dialog.Owner = window;
                dialog.ShowDialog();
            }
            else
            {
                // ucitavanje i otvaranje stranice predloga

                using (var db = new DatabaseContext())
                {
                    //List<Dogadjaj> dogadjajiZaKlijenta = db.Klijenti.Include("Dogadjaji").Include("Dogadjaji.Organizator").SingleOrDefault(k => k.Email.Equals(email)).Dogadjaji;
                    //if (dogadjajiZaKlijenta == null)
                    //{
                    //    dogadjajiZaKlijenta = new List<Dogadjaj>();
                    //}
                    List<Zadatak> zadaci = db.Dogadjaji.Include("Zadaci").
                                                   Include("Zadaci.IzabraniPredlog").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik").
                                                   Include("Zadaci.IzabraniPredlog.Ponuda.Saradnik.Adresa").
                                                   SingleOrDefault(d => d.Id == SelectedDogadjaj.Id).
                                                   Zadaci; 
                    Predlog predlog = zadaci.Find(z => z.Tip == Zadatak.TipZadatka.GLAVNI).IzabraniPredlog;

                    PregledPredlogaViewModel pregledPredlogaViewModel = new PregledPredlogaViewModel(_navigationStore, predlog, zadaci);
                    _navigationStore.CurrentViewModel = pregledPredlogaViewModel;
                }
            }
        }


        public ICommand DetailsCommand
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(_detailsCommand => DetailsEvent());
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
            if (SelectedDogadjaj.Organizator != null)
            {
                detailsModel.Organizator = SelectedDogadjaj.Organizator.Ime + " " + SelectedDogadjaj.Organizator.Prezime;
            }
            detailsModel.DatumOdrzavanja = SelectedDogadjaj.DatumOdrzavanja.ToString("dd/MM/yyyy HH:mm");
            detailsModel.Napomene = SelectedDogadjaj.Napomena;
            detailsModel.MestoOdrzavanja = SelectedDogadjaj.MestoOdrzavanja;
            details.DataContext = detailsModel;
            details.Show();

        }
        public ICommand PovratakCommand
        {
            get
            {
                if (_povratakCommand == null)
                    _povratakCommand = new RelayCommand(_povratakCommand => Povratak());
                return _povratakCommand;
            }
        }

        public void Povratak()
        {
            if (IsKlijent)
            {
                _navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
            }
            else
            {
                _navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
            }
        }
    }
}
