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
	public class PregledPonudaViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;

		private readonly ViewModelBase _previousViewModel;

		private readonly IPonudaService PonudaService = new PonudaService();

		private ObservableCollection<Ponuda> _ponude;

		public ObservableCollection<Ponuda> Ponude
		{
			get => _ponude;
			set
			{
				_ponude = value;
				OnPropertyChanged(nameof(Ponude));
			}
		}

		private Ponuda _izabranaPonuda;

		public Ponuda IzabranaPonuda
		{
			get => _izabranaPonuda;
			set
			{
				_izabranaPonuda = value;
				OnPropertyChanged(nameof(IzabranaPonuda));
			}
		}
		public PregledPonudaViewModel(NavigationStore navigationStore, ViewModelBase viewModelBase)
		{
			_navigationStore = navigationStore;
			_previousViewModel = viewModelBase;
			Ponude = PonudaService.svePonude();

		}

		private ICommand _povratakCommand;

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
			_navigationStore.CurrentViewModel = _previousViewModel;
		}

		private ICommand _deleteCommand;

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(window => DeletePonuda((Window)window));
				return _deleteCommand;
			}
		}

		private ICommand _dodajPonuduCommand;

		public ICommand DodajPonuduCommand
		{
			get
			{
				if (_dodajPonuduCommand == null)
					_dodajPonuduCommand = new RelayCommand(_dodajPonuduCommand => DodajPonuduProzor());
				return _dodajPonuduCommand;
			}
		}

		private void DodajPonuduProzor()
		{
			OrganizatorDodavanjePonude view = new OrganizatorDodavanjePonude(this);
			DodajPonuduViewModel context = new DodajPonuduViewModel();
			view.DataContext = context;
			view.Show();

			Ponude = PonudaService.svePonude();
		}
		private void DeletePonuda(Window window)
		{
			Dialog dialog = new Dialog();
			DialogViewModel viewModel = new DialogViewModel();
			viewModel._message = "Da li želite da obrišete ponudu?";
			dialog.DataContext = viewModel;
			dialog.Owner = window;
			dialog.ShowDialog();


			SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
			SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();


			if (viewModel.odgovor == "Da")
			{
				int id = IzabranaPonuda.Id;
				bool izbrisanaPonuda = Ponude.Remove(IzabranaPonuda);

				if (izbrisanaPonuda)
				{
					try
					{
						PonudaService.RemovePonuda(id);
						dialogModel.IsError = false;
						dialogModel.Message = "Uspešno ste obrisali ponudu!";
						newDialog.DataContext = dialogModel;
						newDialog.Owner = window;
						newDialog.ShowDialog();
					}
					catch (Exception) 
					{
						dialogModel.IsError = true;
						dialogModel.Message = "Desila se greška kod brisanja ponude!";
						newDialog.DataContext = dialogModel;
						newDialog.Owner = window;
						newDialog.ShowDialog();
					}
				}
			}
		}
		private ICommand _izmjenaPonudeCommand;
		public ICommand IzmjenaPonudeCommand
		{
			get
			{
				if (_izmjenaPonudeCommand == null)
					_izmjenaPonudeCommand = new RelayCommand(_izmjenaPonudeCommand => OtvoriIzmjenu());
				return _izmjenaPonudeCommand;
			}
		}
		public void refresh()
		{
			Ponude = PonudaService.svePonude();
		}
		public void OtvoriIzmjenu()
		{
			IzmjenaPonudeView izmena = new IzmjenaPonudeView(this);
			IzmjenaPonudeViewModel izmenaModel = new IzmjenaPonudeViewModel();
			izmenaModel.Id = IzabranaPonuda.Id;
			izmenaModel.Cena = IzabranaPonuda.Cena.ToString();
			izmenaModel.Opis = IzabranaPonuda.Opis;
			izmenaModel.Saradnik = IzabranaPonuda.Saradnik;
			izmena.DataContext = izmenaModel;
			izmena.Show();

		}

		private ICommand _pocetnaStranicaCommand;

		public ICommand PocetnaStranicaCommand
		{
			get
			{
				if (_pocetnaStranicaCommand == null)
					_pocetnaStranicaCommand = new RelayCommand(_pocetnaStranicaCommand => PocetnaStrana());
				return _pocetnaStranicaCommand;
			}
		}

		private void PocetnaStrana()
		{
			KorisnikStore korisnik = KorisnikStore.Instance;
			Korisnik k = korisnik.TrenutniKorisnik;

			if (k.GetType() == typeof(Administrator))
			{
				_navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
			}
			else if (k.GetType() == typeof(Organizator))
			{
				_navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
			}
			else
			{
				_navigationStore.CurrentViewModel = new KlijentHomeViewModel(_navigationStore);
			}
		}
	}
}
