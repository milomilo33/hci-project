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
	public class DodajPonuduViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;

		private readonly ISaradnikService SaradnikService = new SaradnikService();

		private readonly IPonudaService PonudaService = new PonudaService();

		private Saradnik _saradnik;

		public Saradnik Saradnik
		{
			get => _saradnik;
			set 
			{
				_saradnik = value;
				
				OnPropertyChanged(nameof(Saradnik));
			}
		}
		private ObservableCollection<Saradnik> _saradnici;

		public ObservableCollection<Saradnik> Saradnici
		{
			get => _saradnici;
			set
			{
				_saradnici = value;
				OnPropertyChanged(nameof(Saradnici));
			}
		}

		private string _cena;

		public string Cena
		{
			get => _cena;
			set
			{
				_cena = value;
				OnPropertyChanged(nameof(Cena));
			}
		}

		private string _opis;

		public string Opis
		{
			get => _opis;
			set
			{
				_opis = value;
				OnPropertyChanged(nameof(Opis));
			}
		}
		private Ponuda _novaPonuda;

		public Ponuda NovaPonuda
		{
			get => _novaPonuda;
			set 
			{
				_novaPonuda = value;
				OnPropertyChanged(nameof(NovaPonuda));
			}

		}

		public DodajPonuduViewModel(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
			Saradnici = SaradnikService.sviSaradnici();
			
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
			_navigationStore.CurrentViewModel = new PregledPonudaViewModel(_navigationStore);
		}

		private ICommand _dodajCommand;

		public ICommand DodajCommand
		{
			get
			{
				if (_dodajCommand == null)
					_dodajCommand = new RelayCommand(_dodajCommand => DodajPonudu());
				return _dodajCommand;
			}
		}

		private void DodajPonudu()
		{
			NovaPonuda = new Ponuda();
			NovaPonuda.Cena = double.Parse(Cena);
			NovaPonuda.Opis = Opis;
			
			MessageBoxResult result = MessageBox.Show("Da li želite dodati ponudu '" + NovaPonuda.Opis + "'?", "Dodaj ponudu", MessageBoxButton.YesNo);
			switch (result)
			{
				case MessageBoxResult.Yes:
					Ponuda sacuvana = new Ponuda();
					using (var db = new DatabaseContext())
					{
						var resultVar = db.Saradnici.SingleOrDefault(s => s.Id == Saradnik.Id);
						if (resultVar != null)
						{
							NovaPonuda.Saradnik = resultVar;
							sacuvana = db.Ponude.Add(NovaPonuda);
							db.SaveChanges();
						}
					}

					if (sacuvana != null)
					{
						MessageBox.Show("Uspešno ste dodali ponudu!", "Uspeh");
						_navigationStore.CurrentViewModel = new PregledPonudaViewModel(_navigationStore);
					}
					else {
						MessageBox.Show("Ponuda nije mogla da se doda!", "Greska");
					}
					break;

			}
		}
	}
}
