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
		public PregledPonudaViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
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
			_navigationStore.CurrentViewModel = new OrganizatorHomeViewModel(_navigationStore);
		}

		private ICommand _deleteCommand;

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(_deleteCommand => DeletePonuda());
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
			_navigationStore.CurrentViewModel = new DodajPonuduViewModel(_navigationStore);
		}
		private void DeletePonuda()
		{
			
			MessageBoxResult result = MessageBox.Show("Da li želite obrisati ponudu '" + IzabranaPonuda.Opis+ "'?", "Obriši ponudu", MessageBoxButton.YesNo);
			switch (result)
			{
				case MessageBoxResult.Yes:
					int id = IzabranaPonuda.Id;
					bool izbrisanaPonuda = Ponude.Remove(IzabranaPonuda);

					if (izbrisanaPonuda)
					{
						PonudaService.RemovePonuda(id);
						MessageBox.Show("Uspešno ste obrisali ponudu!", "Ponuda obrisana");
					}
					else {
						MessageBox.Show("Ne postoji ponuda!", "Greška");
					}
					
					break;
				
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
	}
}
