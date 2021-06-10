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

namespace Projekat.Views
{
	/// <summary>
	/// Interaction logic for HelpViewer.xaml
	/// </summary>
	public partial class HelpViewer : Window
	{
		private JavaScriptControlHelper ch;
		public HelpViewer(string helpKey, Window originator)
		{
			InitializeComponent();
			Uri u = new Uri($"http://localhost:8080/{GetUrlForHelpKey(helpKey)}");
			ShowHelp(u, originator);

		}
		public HelpViewer(Type dataContextType, Window originator)
		{
            
			InitializeComponent();

			Uri u = new Uri($"http://localhost:8080/{GetUrlForContext(dataContextType)}");
            Console.WriteLine(u.OriginalString);
            ShowHelp(u, originator);
		}
        
		private void ShowHelp(Uri u, Window originator)
		{
			ch = new JavaScriptControlHelper(originator);
			wbHelp.ObjectForScripting = ch;
            Console.WriteLine(u);
			wbHelp.Navigate(u);
		}
		private string GetUrlForHelpKey(string helpKey)
		{
            Console.WriteLine(helpKey);
            var keysToUrlDictionary = new Dictionary<string, string>()
            {
               
				{ "Login", "klijent/prijava.html" }
                
				//{ "Address", "user/additional.html#odabir-adrese" },
				//{ "DateTimePicker", "user/additional.html#odabir-datuma" },
			};

			return keysToUrlDictionary.ContainsKey(helpKey) ? keysToUrlDictionary[helpKey] : "";
		}
        private string GetUrlForContext(Type dataContextType)
        {
            var contextsToUrlDictionary = new Dictionary<Type, string>
            {
                [typeof(RegistrationViewModel)] = "klijent/registracija.html",

                // Client Home
                [typeof(KlijentHomeViewModel)] = "klijent/pocetna-stranica.html#početna-stranica-klijenta",
                // Client Create Celeb Req
                [typeof(KlijentKreiranjeDogadjajaViewModel)] = "klijent/kreiranje-dogadjaja.html#kreiranje-događaja",
                [typeof(KlijentKreiranjeDogadjajaOdabirOrganizatoraViewModel)] = "klijent/kreiranje-dogadjaja.html#odabir-organizatora",
                [typeof(KlijentPregledOrganizatoraViewModel)] = "klijent/pregled-organizatora.html#prikaz-organizatora",
                [typeof(KomunikacijaViewModel)] = "klijent/pregled-dogadjaja.html#poruke",
                // Client Celeb Proposals
                [typeof(KreiranjeZadatkaViewModel)] = "organizator/dodeljeni-dogadjaji.html#dodavanje-zadataka",
                [typeof(LoginViewModel)] = "klijent/prijava.html#prijava",
                [typeof(OrganizatorDodeljeniDogadjajiViewModel)] = "organizator/dodeljeni-dogadjaji.html",

                // Organizer Home
                [typeof(OrganizatorEventTableViewModel)] = "organizator/pregled-dogadjaja.html",
                [typeof(OrganizatorHomeViewModel)] = "organizator/pocetna-stranica.html",
                [typeof(OrganizatorProfilViewModel)] = "organizator/profil.html",
                [typeof(PredloziZaZadatakViewModel)] = "organizator/dodeljeni-dogadjaji.html#prihvatanje-ponuda",
                [typeof(PregledPonudaViewModel)] = "organizator/pregled-ponuda.html",


                // Admin Home
                [typeof(AdminHomeViewModel)] = "administrator/pocetna-stranica.html",
                [typeof(PregledPredlogaViewModel)] = "klijent/pregled-dogadjaja.html#pikaz-predloga",
                [typeof(PregledPredlogaDodatniZahteviViewModel)] = "klijent/pregled-dogadjaja.html#prikaz-predloga",
                [typeof(RasporedSedenjaViewModel)] = "klijent/pregled-dogadjaja.html#raspored-sedenja",
                [typeof(TaskViewModel)] = "organizator/dodeljeni-dogadjaji.html#zadaci-događaja",
                // Celebrations HERE
               // [typeof(AdminPonudeEventTableViewModel)] = "admin/collaborators.html#kreiranje-saradnika",
                [typeof(AdminPregledKlijenataViewModel)] = "administrator/pregled-klijenata.html",
                [typeof(AdminPregledOrganizatoraViewModel)] = "administrator/pregled-organizatora.html",
                [typeof(DetailsViewModel)] = "administrator/pregled-dogadjaja.html#detalji-događaja",
                [typeof(DodajGostaDialogViewModel)] = "klijent/collaborators.html#dodavanje-gosta",
                [typeof(DodajPonuduViewModel)] = "organizator/pregled-ponuda.html#dodavanje-ponude",
                [typeof(EventListViewModel)] = "administrator/pregled-dogadjaja.html",
                [typeof(IzmenaZadatkaViewModel)] = "organizator/dodeljeni-dogadjaji.html#izmena-zadatka",
                [typeof(AddOrganizatorViewModel)] = "administrator/pregled-organizatora.html#dodavanje-organizatora",
                [typeof(AddSaradnikViewModel)] = "administrator/pregled-saradnika.html#dodavanje-saradnika",
               
            };

            return contextsToUrlDictionary.ContainsKey(dataContextType) ? contextsToUrlDictionary[dataContextType] : "";
        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (wbHelp != null) && wbHelp.CanGoBack;
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (wbHelp != null) && wbHelp.CanGoForward;
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoForward();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

    }
}
