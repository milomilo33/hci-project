using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Projekat.ViewModels.IzmjenaPonudeViewModel;

namespace Projekat.Views
{
	/// <summary>
	/// Interaction logic for IzmjenaPonudeView.xaml
	/// </summary>
	public partial class IzmjenaPonudeView : Window
	{
		public PregledPonudaViewModel Next;
		public IzmjenaPonudeView(PregledPonudaViewModel next)
		{
			InitializeComponent();
			Loaded += CreateTask_Loaded;
			Next = next;
		}

		private void CreateTask_Loaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is ICloseWindow vm)
			{
				vm.Close += () =>
				{
					this.Close();
					Next.refresh();
				};
			}
		}
	}
}
