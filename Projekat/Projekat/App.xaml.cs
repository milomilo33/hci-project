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


            
           /*
            using (var db = new DatabaseContext())
            {
                 Adresa adr = new Adresa();
                 adr.Broj = 69;
                 adr.Grad = "Novi Sad";
                 adr.Ulica = "Bulevar oslobođenja";
                 adr.Drzava = "Srbija";
                 db.Adrese.Add(adr);

                 Administrator a = new Administrator();
                 a.BrojTelefona = "069222222";
                 a.Email = "example@mail.com";
                 a.Ime = "Nikola";
                 a.Prezime = "Nikolic";
                 a.Lozinka = "123";
                 a.Adresa = adr;
                 db.Administratori.Add(a);

                 Organizator o = new Organizator();
                 o.BrojTelefona = "069222222";
                 o.Email = "organizator@mail.com";
                 o.Ime = "Nikola";
                 o.Prezime = "Nikolic";
                 o.Lozinka = "123";
                 o.Dogadjaji = new List<Dogadjaj>();
                 o.Adresa = adr;


                 Organizator o2 = new Organizator();
                 o2.BrojTelefona = "069222222";
                 o2.Email = "organizator2@mail.com";
                 o2.Ime = "Jovan";
                 o2.Prezime = "Markovic";
                 o2.Lozinka = "123";
                 o2.Dogadjaji = new List<Dogadjaj>();
                 o2.Adresa = adr;
                 

                 Organizator o3 = new Organizator();
                 o3.BrojTelefona = "069222222";
                 o3.Email = "organizator3@mail.com";
                 o3.Ime = "Petar";
                 o3.Prezime = "Milic";
                 o3.Lozinka = "123";
                 o3.Dogadjaji = new List<Dogadjaj>();
                 o3.Adresa = adr;
               
                 Organizator o4 = new Organizator();
                 o4.BrojTelefona = "069222222";
                 o4.Email = "organizator4@mail.com";
                 o4.Ime = "Ana";
                 o4.Prezime = "Jovic";
                 o4.Lozinka = "123";
                 o4.Dogadjaji = new List<Dogadjaj>();
                 o4.Adresa = adr;
                 

                 Zadatak z1 = new Zadatak();
                 z1.Id = 1;

                 z1.Naziv = "Restoran";
                 z1.Opis = "Pronaći restoran";
                 z1.Status = "U obradi";

                 z1.Glavni = true;

                 Zadatak z2 = new Zadatak();
                 z2.Id = 2;

                 z2.Naziv = "Cveće";
                 z2.Opis = "Pronaći cvećaru";
                 z2.Status = "U obradi";


               


                 Zadatak z3 = new Zadatak();
                 z3.Id = 3;
                 
                 z3.Naziv = "Restoran";
                 z3.Opis = "Pronaći restoran";
                 z3.Status = "U obradi";



                 Zadatak z4 = new Zadatak();
                 z4.Id = 4;

                 z4.Naziv = "Cveće";
                 z4.Opis = "Pronaći cvećaru";
                 z4.Status = "U obradi";


                Ponuda p1 = new Ponuda();
                p1.Opis = "Nudimo vam salu sa 200 stolica, cena po stolici 10e";
                p1.Cena = 20000;
                p1.Nevazeca = false;
                db.Ponude.Add(p1);
                
                Ponuda p2 = new Ponuda();
                p2.Opis = "Prostor za izdavanje - jedan dan 100e";
                p2.Cena = 60000;
                p2.Nevazeca = false;
                db.Ponude.Add(p2);

                 



                 Klijent k = new Klijent();
                 k.BrojTelefona = "069222222";
                 k.Email = "klijent@mail.com";
                 k.Ime = "Nikola";
                 k.Prezime = "Nikolic";
                 k.Lozinka = "123";
                 k.Dogadjaji = new List<Dogadjaj>();
                 k.Adresa = adr;
                 db.Klijenti.Add(k);

                 Saradnik s1 = new Saradnik();
                 s1.Naziv = "Borsalino";
                 s1.Opis = "Nudimo vam izdaju prostora kao i obezbjedjen katering";
                 s1.Tip = "Restoran";

                 List<Ponuda> ponude = new List<Ponuda>();
                 ponude.Add(p1);
                 ponude.Add(p2);
                 s1.Ponude = ponude;

                 db.Saradnici.Add(s1);

                 p2.Saradnik = s1;
                 p1.Saradnik = s1;
                 db.Ponude.Add(p1);
                 db.Ponude.Add(p2); 

                 Dogadjaj d1 = new Dogadjaj();
                 d1.Id = 1;
                 d1.MestoOdrzavanja = "Bašta";
                 d1.Napomena = "Nema napomena";
                 d1.Organizator = o;
                 d1.Status = "Na čekanju";
                 d1.VrstaProslave = "18. rodjendan";
                 d1.Tema = "Vrh";
                 d1.BudzetPromenljiv = true;
                 d1.BrojGostiju = 150;
                 d1.DatumOdrzavanja = DateTime.Now;
                 d1.Budzet = 30000;
                 d1.DodatniZahtevi = "nema";


                 d1.Zadaci = new List<Zadatak>();


                 Dogadjaj d2 = new Dogadjaj();
                 d2.Id = 2;
                 d2.MestoOdrzavanja = "Sala";
                 d2.Napomena = "Nema napomena";
                 d2.Organizator = o;
                 d2.Status = "Na čekanju";
                 d2.VrstaProslave = "Svadba";
                 d2.Tema = "Top";
                 d2.BudzetPromenljiv = true;
                 d2.BrojGostiju = 500;
                 d2.DatumOdrzavanja = DateTime.Now;
                 d2.Budzet = 150000;
                 d2.DodatniZahtevi = "nema";


                 d2.Zadaci = new List<Zadatak>();

                 d1.Zadaci.Add(z1);
                 d1.Zadaci.Add(z2);

                 d2.Zadaci.Add(z3);
                 d2.Zadaci.Add(z4);

                 z3.Dogadjaj = d2;
                 z4.Dogadjaj = d2;

                 db.Zadaci.Add(z1);
                 db.Zadaci.Add(z2);
                 db.Zadaci.Add(z3);
                 db.Zadaci.Add(z4);
                db.Organizatori.Add(o);
                db.Organizatori.Add(o2);
                db.Organizatori.Add(o3);
                db.Organizatori.Add(o4);
                db.Dogadjaji.Add(d1);
                db.Dogadjaji.Add(d2);
                     db.SaveChanges();

            





            }  
            */



            base.OnStartup(e);

        }
    }
}
