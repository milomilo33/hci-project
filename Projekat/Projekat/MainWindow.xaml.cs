using Microsoft.Win32;
using Projekat.Stores;
using Projekat.ViewModels;
using Projekat.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var pricipal = new System.Security.Principal.WindowsPrincipal(
                            System.Security.Principal.WindowsIdentity.GetCurrent());
            if (pricipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                RegistryKey registrybrowser = Registry.LocalMachine.OpenSubKey
                (@"Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                var currentValue = registrybrowser.GetValue("*");
                if (currentValue == null || (int)currentValue != 0x00002af9)
                    registrybrowser.SetValue("*", 0x00002af9, RegistryValueKind.DWord);
            }
            else
                this.Title += " (Mora se pokrenuti aplikacija kao administrator)";
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
               
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (str != null)
                {
                    HelpProvider.ShowHelp(str, this);
                    return;
                }
			}
           
			var context =((MainViewModel) this.DataContext).CurrentViewModel;
           

            var helpViewer = context is ViewModelBase navigationModelView ? new HelpViewer(navigationModelView.GetType(), this)
                : new HelpViewer(context.GetType(), this);

            helpViewer.Show();
        }
    }
}
