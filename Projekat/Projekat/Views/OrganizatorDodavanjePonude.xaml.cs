using Projekat.Model;
using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Projekat.ViewModels.DodajPonuduViewModel;

namespace Projekat.Views
{
	/// <summary>
	/// Interaction logic for OrganizatorDodavanjePonude.xaml
	/// </summary>
	public partial class OrganizatorDodavanjePonude : Window
	{
		public PregledPonudaViewModel Next;
		public OrganizatorDodavanjePonude(PregledPonudaViewModel next)
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

        public Ponuda PonudaTutorial { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            PonudaTutorial = new Ponuda
            {
                Cena = 2499,
                Opis = "Ovo je opis ponude"
            };

            TutorialThread = new Thread(CallTutorialMethods);
            TutorialThread.Start();
        }

        public void StopTutorial(object sender, RoutedEventArgs e)
        {
            try
            {
                TutorialThread.Abort();
                // resetComponents();
                enableComponents();
                clearComponents();
                StopTutorialBtn.Visibility = System.Windows.Visibility.Hidden;
                AddBtn.Background = Brushes.Orange;
            }
            catch (Exception ex) { }
        }

        private void enableComponents()
        {
            AddBtn.IsEnabled = true;
            CloseBtn.IsEnabled = true;
            SaradnikCB.IsEnabled = false;
            textName1.IsReadOnly = false;
            textName2.IsReadOnly = false;
        }

        private void CallTutorialMethods()
        {
            while (true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();
                textBoxDemo(textName1, PonudaTutorial.Cena.ToString());
                textBoxDemo(textName2, PonudaTutorial.Opis);
                comboBoxDemo(SaradnikCB);
                buttonDemo(AddBtn);
            }
        }

        private void comboBoxDemo(ComboBox comboBox)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    comboBox.IsDropDownOpen = true;
                }));
                Thread.Sleep(450);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    comboBox.SelectedIndex = 0;
                    comboBox.IsDropDownOpen = false;
                }));
            }
            catch (Exception ex) { }
        }

        private void buttonDemo(Button button)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    button.IsEnabled = true;
                    button.Background = Brushes.GreenYellow;
                }));
                Thread.Sleep(300);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    BrushConverter bc = new BrushConverter();
                    button.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#4267B2");
                    button.IsEnabled = false;
                }));
            }
            catch (Exception ex) { }
        }
        private void textBoxDemo(TextBox textBox, string value)
        {
            try
            {
                for (int i = 1; i <= value.Length; i++)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        textBox.Text = value.Substring(0, i);
                    }));
                    Thread.Sleep(150);
                }
            }
            catch (Exception ex) { }
        }

        private void clearComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() => {
                    textName2.Text = "";
                    textName1.Text = "";
                    SaradnikCB.SelectedIndex = -1;
                }));
            }
            catch (Exception e) { }
        }

        private void DisableComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    textName1.IsReadOnly = true;
                    textName2.IsReadOnly = true;
                    SaradnikCB.IsEnabled = false;
                    AddBtn.IsEnabled = false;
                    CloseBtn.IsEnabled = false;
                }));
            }
            catch (Exception e) { }
        }

        private void ToggleStopVisibility()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    StopTutorialBtn.Visibility = System.Windows.Visibility.Visible;
                }));
            }
            catch (Exception ex) { }
        }
    }
}
