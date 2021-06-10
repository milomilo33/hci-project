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
    /// Interaction logic for CreateTask.xaml
    /// </summary>
    public partial class CreateTask : Window
    {
        public TaskViewModel Next;
        public CreateTask(TaskViewModel next)
        {
            InitializeComponent();
            Loaded += CreateTask_Loaded;
            Next = next;
        }

        private void CreateTask_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is ICloseWindow  vm)
            {
                vm.Close += () =>
                {
                    this.Close();
                    Next.refresh();
                };
            }

        }

        public Zadatak ZadatakTutorial { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            ZadatakTutorial = new Zadatak
            {
                Naziv = "Zadatak 1",
                Tip = Zadatak.TipZadatka.DODATNI,
                Opis = "Ovaj tekst predstavlja opis zadatka"
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
                StopTutorailBtn.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex) { }
        }

        private void enableComponents()
        {
            AddBtn.IsEnabled = true;
            TipCB.IsEnabled = true;
            NazivTB.IsReadOnly = false;
            OpisTB.IsReadOnly = false;
        }

        private void CallTutorialMethods()
        {
            while(true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();
                textBoxDemo(NazivTB, ZadatakTutorial.Naziv);
                comboBoxDemo(TipCB);
                textBoxDemo(OpisTB, ZadatakTutorial.Opis);
                buttonDemo(AddBtn);
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
            try {
                this.Dispatcher.Invoke((Action)(() => {
                    NazivTB.Text = "";
                    OpisTB.Text = "";
                    TipCB.SelectedIndex = -1; 
                }));
            } catch (Exception e) { }
        }

        private void DisableComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NazivTB.IsReadOnly = true;
                    OpisTB.IsReadOnly = true;
                    TipCB.IsEnabled = false;
                    AddBtn.IsEnabled = false;
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
                    StopTutorailBtn.Visibility = System.Windows.Visibility.Visible;
                }));
            }
            catch (Exception ex) { }
        }
    }
}
