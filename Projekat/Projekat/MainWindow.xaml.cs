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
