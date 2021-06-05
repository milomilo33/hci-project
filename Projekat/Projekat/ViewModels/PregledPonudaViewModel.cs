using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Service;
using Projekat.Stores;
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
			DogadjajService d = new DogadjajService();
			Ponude = PonudaService.svePonude();

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
	}
}
