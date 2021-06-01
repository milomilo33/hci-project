using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }

        public DbSet<Organizator> Organizatori { get; set; }

        public DbSet<Klijent> Klijenti { get; set; }

        public DbSet<Administrator> Administratori { get; set; }

        public DbSet<Adresa> Adrese { get; set; }

        public DbSet<Dogadjaj> Dogadjaji { get; set; }

        public DbSet<Komentar> Komentari { get; set; }

        public DbSet<Ponuda> Ponude { get; set; }

        public DbSet<Saradnik> Saradnici { get; set; }

        public DbSet<Zadatak> Zadaci { get; set; }
    }
}
