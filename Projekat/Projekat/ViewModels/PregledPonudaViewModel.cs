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
		public PregledPonudaViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
			DogadjajService d = new DogadjajService();
			Ponude = PonudaService.svePonude();

		}
	}
}
