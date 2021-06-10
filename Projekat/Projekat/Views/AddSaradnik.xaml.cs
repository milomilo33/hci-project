using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Projekat.Model;
using Projekat.Stores;
using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AddSaradnik.xaml
    /// </summary>
    public partial class AddSaradnik : Window
    {
        public AdminPregledSaradnikaViewModel Next;

        private (double, double) coordinates { get; set; }
        public AddSaradnik(AdminPregledSaradnikaViewModel next)
        {
            InitializeComponent();
            Loaded += CreateAddOrganizator_Loaded;
            Next = next;
        }
        private void CreateAddOrganizator_Loaded(object sender, RoutedEventArgs e)
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

        public void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Address_Updated(object sender, RoutedEventArgs e)
        {
            coordinates = (DataContext as AddSaradnikViewModel).GetLocationFromAddressAsync();
        }

        private void Load_On_Map(object sender, RoutedEventArgs e)
        {
            MapBtn.IsEnabled = false;
            MapControl_Loaded(null, null);


            if (coordinates.Item1 == -1 && coordinates.Item2 == -1)
            {
                SuccessOrErrorDialog newDialog = new SuccessOrErrorDialog();
                SuccessOrErrorDialogViewModel dialogModel = new SuccessOrErrorDialogViewModel();
                dialogModel.IsError = true;
                dialogModel.Message = "Adresa koju ste uneli ne postoji.";
                newDialog.DataContext = dialogModel;
                newDialog.ShowDialog();
                return;
            }
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {

            coordinates = (DataContext as AddSaradnikViewModel).GetLocationFromAddressAsync();

            if (coordinates.Item1 == -1 && coordinates.Item2 == -1)
            {

                return;
            }
            // Specify a known location.


            var position = new BasicGeoposition() { Latitude = coordinates.Item1, Longitude = coordinates.Item2 };
            var center = new Geopoint(position);

            if (center == null) return;
            // Set the map location.

            await (this.FindName("mapControl") as MapControl).TrySetViewAsync(center, 15);
            MapBtn.IsEnabled = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null) return;
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                (DataContext as AddSaradnikViewModel).IsLokal = true;
                (DataContext as AddSaradnikViewModel).IsOther = false;
            }
            else if ((sender as ComboBox).SelectedIndex == 1)
            {
                (DataContext as AddSaradnikViewModel).IsOther = true;
                (DataContext as AddSaradnikViewModel).IsLokal = false;
            }
        }

        public Saradnik SaradnikTutorial { get; set; }
        public Saradnik TempSaradnik { get; set; }
        public Thread TutorialThread { get; set; }
        public void ExecuteTutorial(object sender, RoutedEventArgs e)
        {
            Adresa adresaTutorial = new Adresa
            {
                Ulica = "Narodnog fronta",
                Broj = 11,
                Grad = "Novi Sad",
                Drzava = "Srbija"
            };

            SaradnikTutorial = new Saradnik
            {
                Naziv = "Iris",
                Opis = "Cvećara Iris",
                Tip = "Cvećara",
                BrojTelefona = "0643425623",
                Adresa = adresaTutorial
            };

            
            Adresa tempAdresa = new Adresa
            {
                Ulica = UlicaTB.Text,
                Grad = GradTB.Text,
                Drzava = DrzavaTB.Text
            };

            int.TryParse(BrojTB.Text, out int broj);
            tempAdresa.Broj = broj;
         
            TempSaradnik = new Saradnik
            {
                Naziv = NazivTB.Text,
                Opis = OpisTB.Text,
                Adresa = tempAdresa,
                BrojTelefona = BrojTelefonaTB.Text
            };

            if(TipCB.Text == "Lokal")
            {
                TempSaradnik.Tip = "Lokal";
            }
            else
            {
                TempSaradnik.Tip = TipTB.Text;
            }



            TutorialThread = new Thread(CallTutorialMehotds);
            TutorialThread.Start();
        }

        private void CallTutorialMehotds()
        {
            while (true)
            {
                ToggleStopVisibility();
                DisableComponents();
                clearComponents();
                textBoxDemo(NazivTB, SaradnikTutorial.Naziv);
                textBoxDemo(OpisTB, SaradnikTutorial.Opis);
                comboBoxDemo(TipCB);
                textBoxDemo(TipTB, SaradnikTutorial.Tip);
                textBoxDemo(BrojTelefonaTB, SaradnikTutorial.BrojTelefona);
                textBoxDemo(UlicaTB, SaradnikTutorial.Adresa.Ulica);
                textBoxDemo(BrojTB, SaradnikTutorial.Adresa.Broj.ToString());
                textBoxDemo(GradTB, SaradnikTutorial.Adresa.Grad);
                textBoxDemo(DrzavaTB, SaradnikTutorial.Adresa.Drzava);
                buttonDemo(MapBtn);
                this.Dispatcher.InvokeAsync(() => {
                    var position = new BasicGeoposition() { Latitude = 45.2418338, Longitude = 19.8445078 };
                    var center = new Geopoint(position);

                    
                    if(mapControl != null)
                        mapControl.TrySetViewAsync(center, 15);
                });
                Thread.Sleep(3500);
                buttonDemo(AddBtn);
            }
        }

        private void clearComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NazivTB.Text = "";
                    OpisTB.Text = "";
                    TipTB.Text = "";
                    BrojTelefonaTB.Text = "";
                    UlicaTB.Text = "";
                    GradTB.Text = "";
                    DrzavaTB.Text = "";
                    BrojTB.Text = "";
                    TipCB.SelectedIndex = -1;
                    var position = new BasicGeoposition() { Latitude = 0, Longitude = 0 };
                    var center = new Geopoint(position);

                    (this.FindName("mapControl") as MapControl).TrySetViewAsync(center, 0);
                }));
            }
            catch (Exception e) { }
        }

        private void resetComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NazivTB.Text = TempSaradnik.Naziv;
                    OpisTB.Text = TempSaradnik.Opis;
                    TipTB.Text = TempSaradnik.Tip;
                    BrojTelefonaTB.Text = TempSaradnik.BrojTelefona;
                    UlicaTB.Text = TempSaradnik.Adresa.Ulica;
                    GradTB.Text = TempSaradnik.Adresa.Grad;
                    DrzavaTB.Text = TempSaradnik.Adresa.Drzava;
                    BrojTB.Text = TempSaradnik.Adresa.Broj.ToString();
                    TipCB.SelectedIndex = -1;
                    var position = new BasicGeoposition() { Latitude = 0, Longitude = 0 };
                    var center = new Geopoint(position);
                    
                    (this.FindName("mapControl") as MapControl).TrySetViewAsync(center, 0);
                }));
            } catch(Exception e) { }
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
                    comboBox.SelectedIndex = 1;
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

        private void DisableComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NazivTB.IsReadOnly = true;
                    OpisTB.IsReadOnly = true;
                    TipTB.IsReadOnly = true;
                    BrojTelefonaTB.IsReadOnly = true;
                    UlicaTB.IsReadOnly = true;
                    BrojTB.IsReadOnly = true;
                    GradTB.IsReadOnly = true;
                    DrzavaTB.IsReadOnly = true;
                    TipCB.IsEnabled = false;
                    MapBtn.IsEnabled = false;
                    CancelBtn.IsEnabled = false;
                    AddBtn.IsEnabled = false;
                    EditBtn.IsEnabled = false;
                }));
            } catch(Exception e) { }
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

        private void StopDemoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TutorialThread.Abort();
                resetComponents();
                enableComponents();
                StopTutorialBtn.Visibility = System.Windows.Visibility.Hidden;
                AddBtn.Background = Brushes.Orange;
                MapBtn.Background = Brushes.White;
            }
            catch (Exception ex) { }

            
        }

        private void enableComponents()
        {
            NazivTB.IsReadOnly = false;
            OpisTB.IsReadOnly = false;
            TipTB.IsReadOnly = false;
            BrojTelefonaTB.IsReadOnly = false;
            UlicaTB.IsReadOnly = false;
            BrojTB.IsReadOnly = false;
            GradTB.IsReadOnly = false;
            DrzavaTB.IsReadOnly = false;
            TipCB.IsEnabled = true;
            MapBtn.IsEnabled = true;
            CancelBtn.IsEnabled = true;
            AddBtn.IsEnabled = true;
            EditBtn.IsEnabled = true;
        }
    }
}
