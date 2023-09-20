using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Projekat.ViewModels
{
	public class AdminPregledKlijenataViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;

		public ObservableCollection<Klijent> Klijenti { get; set; }
		public AdminPregledKlijenataViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;

			using (var db = new DatabaseContext()) {
				Klijenti = new ObservableCollection<Klijent>(db.Klijenti.Include("Adresa"));
				OnPropertyChanged(nameof(Klijenti));
			}
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
			_navigationStore.CurrentViewModel = new AdminHomeViewModel(_navigationStore);
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

		private ICommand _odjavaCommand;
		public ICommand OdjavaCommand
		{
			get
			{
				if (_odjavaCommand == null)
					_odjavaCommand = new RelayCommand(_odjavaCommand => Odjava());
				return _odjavaCommand;
			}
		}

		public void Odjava()
		{
			_navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);

		}
	}
}
