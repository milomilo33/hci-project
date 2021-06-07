using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
