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
using static Projekat.ViewModels.DodajPonuduViewModel;

namespace Projekat.ViewModels
{
	public class DodajPonuduViewModel : ViewModelBase, ICloseWindow
	{
		
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
		public Action Close { get; set; }
		public DodajPonuduViewModel()
		{
			
			
			Saradnici = SaradnikService.sviSaradnici();
			
		}

		

		private ICommand _odustaniCommand;
		public ICommand OdustaniCommand
		{
			get
			{
				if (_odustaniCommand == null)
					_odustaniCommand = new RelayCommand(window => Odustani((Window)window));
				return _odustaniCommand;
			}
		}

		private void Odustani(Window window)
		{

			Dialog dialog = new Dialog();
			DialogViewModel viewModel = new DialogViewModel();
			viewModel._message = "Da li želite da odustanete od dodavanje nove ponude?";
			dialog.DataContext = viewModel;
			dialog.Owner = window;
			dialog.ShowDialog();

			if (viewModel.odgovor == "Da")
			{
				CloseWindow();
			}
		}

		private ICommand _dodajCommand;

		public ICommand DodajCommand
		{
			get
			{
				if (_dodajCommand == null)
					_dodajCommand = new RelayCommand(window => DodajPonudu((Window)window));
				return _dodajCommand;
			}
		}

		private void DodajPonudu(Window window)
		{
			string validationMessage = ValidationMessage();

			if (string.IsNullOrWhiteSpace(validationMessage))
			{
				Dialog dialog = new Dialog();
				DialogViewModel viewModel = new DialogViewModel();
				viewModel._message = "Da li želite da dodate novu ponudu?";
				dialog.DataContext = viewModel;
				dialog.Owner = window;
				dialog.ShowDialog();

				if (viewModel.odgovor == "Da")
				{
					NovaPonuda = new Ponuda();
					NovaPonuda.Cena = double.Parse(Cena);
					NovaPonuda.Opis = Opis;

					Ponuda sacuvana = new Ponuda();
					using (var db = new DatabaseContext())
					{
						var resultVar = db.Saradnici.SingleOrDefault(s => s.Id == Saradnik.Id);
						if (resultVar != null)
						{
							NovaPonuda.Saradnik = resultVar;
							sacuvana = db.Ponude.Add(NovaPonuda);
							db.SaveChanges();

							SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
							SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();

							if (sacuvana != null)
							{
								dialogModel.IsError = false;
								dialogModel.Message = "Uspešno ste dodali ponudu!";
								newDialog.DataContext = dialogModel;
								newDialog.Owner = window;
								newDialog.ShowDialog();
								Console.WriteLine("neeeeeeeeeeeeeeeee");
								CloseWindow();
							}
							else
							{
								dialogModel.IsError = true;
								dialogModel.Message = "Desila se greška kod dodavanja nove ponude!";
								newDialog.DataContext = dialogModel;
								newDialog.Owner = window;
								newDialog.ShowDialog();
							}
						}
					}
				}
			}
			else
			{
				SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
				SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();

				dialogModel.IsError = true;
				dialogModel.Message = validationMessage;
				newDialog.DataContext = dialogModel;
				newDialog.Owner = window;
				newDialog.ShowDialog();
			}
		}

		private string ValidationMessage()
		{
			string message = "";

			if (string.IsNullOrWhiteSpace(Opis))
			{
				message += "Morate napisati opis ponude!\n\n";
			}
			
			if (double.Parse(Cena) <= 0.0)
			{
				message += "Cena ponude mora biti validna, veća od 0!\n\n";
			}
			if (Saradnik == null)
			{
				message += "Morate odabrati saradnika kome dodeljujete ponudu!\n\n";
			}

			return message;
		}

		
		private void CloseWindow()
		{
			Close?.Invoke();
		}

		public interface ICloseWindow
		{
			Action Close { get; set; }
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
			//_navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);

		}
	}
}
