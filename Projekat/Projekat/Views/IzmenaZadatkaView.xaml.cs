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
using static Projekat.ViewModels.KreiranjeZadatkaViewModel;

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for IzmenaZadatkaView.xaml
    /// </summary>
    public partial class IzmenaZadatkaView : Window
    {
        public TaskViewModel Next;
        public IzmenaZadatkaView(TaskViewModel next)
        {
            Next = next;
            InitializeComponent();
            Loaded += Izmena_Loaded;
        }

        private void Izmena_Loaded(object sender, RoutedEventArgs e)
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

        public Zadatak ZadatakTutorial { get; set; }
        public Zadatak TempZadatak { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            TempZadatak = new Zadatak
            {
                Naziv = NazivTB.Text,
                Opis = OpisTB.Text
            };

            ZadatakTutorial = new Zadatak
            {
                Naziv = "Zadatak 1",
                Tip = Zadatak.TipZadatka.DODATNI,
                Opis = "Ovaj tekst predstavlja opis zadatka"
            };

            TutorialThread = new Thread(CallTutorialMethods);
            TutorialThread.Start();
        }


        private void resetComponents()
        {
            NazivTB.Text = TempZadatak.Naziv;
            OpisTB.Text = TempZadatak.Opis;
        }

        private void enableComponents()
        {
            IzmeniBtn.IsEnabled = true;
            NazivTB.IsReadOnly = false;
            OpisTB.IsReadOnly = false;
            OdustaniBtn.IsEnabled = true;
        }

        private void CallTutorialMethods()
        {
            while (true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();
                textBoxDemo(NazivTB, ZadatakTutorial.Naziv);
                textBoxDemo(OpisTB, ZadatakTutorial.Opis);
                buttonDemo(IzmeniBtn);
            }
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
                    NazivTB.Text = "";
                    OpisTB.Text = "";
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
                    NazivTB.IsReadOnly = true;
                    OpisTB.IsReadOnly = true;
                    OdustaniBtn.IsEnabled = false;
                    IzmeniBtn.IsEnabled = false;
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


        private void StopTutorialBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TutorialThread.Abort();
                resetComponents();
                enableComponents();
                StopTutorialBtn.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex) { }
        }

    }
}
