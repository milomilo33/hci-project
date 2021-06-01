using Projekat.Data;
using Projekat.Model;
using Projekat.Stores;
using Projekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new LoginViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            //using (var db = new DatabaseContext())
            //{
            //    //Administrator a = new Administrator();
            //    //a.BrojTelefona = "069222222";
            //    //a.Email = "example@mail.com";
            //    //a.Ime = "Nikola";
            //    //a.Prezime = "Nikolic";
            //    //a.Lozinka = "123";
            //    //db.Administratori.Add(a);
            //    //db.SaveChanges();

            //    //Organizator a = new Organizator();
            //    //a.BrojTelefona = "069222222";
            //    //a.Email = "organizator@mail.com";
            //    //a.Ime = "Nikola";
            //    //a.Prezime = "Nikolic";
            //    //a.Lozinka = "123";
            //    //a.Dogadjaji = new List<Dogadjaj>();
            //    //db.Organizatori.Add(a);
            //    //db.SaveChanges();
            //}

            base.OnStartup(e);
        }
    }
}
