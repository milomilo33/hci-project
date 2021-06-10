using Projekat.Model;
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

namespace Projekat.Views
{
    /// <summary>
    /// Interaction logic for DodajGostaDialogView.xaml
    /// </summary>
    public partial class DodajGostaDialogView : Window
    {
        public DodajGostaDialogView()
        {
            InitializeComponent();
        }

        public Gost GostTutorial { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            GostTutorial = new Gost
            {
                ImeIPrezime = "Gost Gostovski"
            };

            TutorialThread = new Thread(CallTutorialMethods);
            TutorialThread.Start();
        }

        public void StopTutorial(object sender, RoutedEventArgs e)
        {
            try
            {
                TutorialThread.Abort();
                DodajGostaBtn.Background = Brushes.Orange;
                // resetComponents();
                enableComponents();
                clearComponents();
                StopTutorialBtn.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex) { }
        }

        private void enableComponents()
        {
            DodajGostaBtn.IsEnabled = true;
            OdustaniBtn.IsEnabled = true;
            ImeGostaTB.IsReadOnly = false;
        }

        private void CallTutorialMethods()
        {
            while (true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();
                textBoxDemo(ImeGostaTB, GostTutorial.ImeIPrezime);
                buttonDemo(DodajGostaBtn);
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
                    ImeGostaTB.Text = "";
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
                    ImeGostaTB.IsReadOnly = true;
                    OdustaniBtn.IsEnabled = false;
                    DodajGostaBtn.IsEnabled = false;
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
