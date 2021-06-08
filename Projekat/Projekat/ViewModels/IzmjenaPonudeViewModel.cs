using Projekat.Commands;
using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModels
{
	public class IzmjenaPonudeViewModel : ViewModelBase
	{
		

		public IzmjenaPonudeViewModel()
		{
		}

		private ICommand _izmjenaCommand;

		public ICommand IzmjenaCommand
		{
			get
			{
				if (_izmjenaCommand == null)
					_izmjenaCommand = new RelayCommand(_izmjenaCommand => Izmjeni());
				return _izmjenaCommand;
			}
		}

		private void Izmjeni()
		{
			using (var db = new DatabaseContext()) {
				var resultVar = db.Ponude.SingleOrDefault(p => p.Id == Id);
				if (resultVar != null)
				{
					if (resultVar.Cena == double.Parse(Cena))
					{
						MessageBox.Show("Niste izmenili cenu!");
					}
					else {
						resultVar.Cena = double.Parse(Cena);
					}
					
					db.SaveChanges();
					MessageBox.Show("Uspešno ste izmenili ponudu");
				}
			}
		}


		private ICommand _odustaniCommand;

		public ICommand OdustaniCommand
		{
			get
			{
				if (_odustaniCommand == null)
					_odustaniCommand = new RelayCommand(_odustaniCommand => Odustani());
				return _odustaniCommand;
			}
		}

		private void Odustani()
		{


			MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da odustanete od menjanja ponude?", "Izmena ponude", MessageBoxButton.YesNo);
			switch (result)
			{
				case MessageBoxResult.Yes:
					CloseWindow();
					break;

			}
		}
		public Action Close { get; set; }

		private void CloseWindow()
		{
			Close?.Invoke();
		}

		public interface ICloseWindow
		{
			Action Close { get; set; }
		}

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
		private int _id;

		public int Id
		{
			get => _id;
			set
			{
				_id = value;
				OnPropertyChanged(nameof(Id));

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
	}
}

