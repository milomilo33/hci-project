using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Stores
{
    public sealed class KorisnikStore
    {
        private static KorisnikStore _instance = null;

        private KorisnikStore()
        {

        }

        public static KorisnikStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KorisnikStore();
                return _instance;
            }
        }

        public Korisnik TrenutniKorisnik { get; set; }
    }
}
