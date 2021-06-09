using Projekat.Data;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Service
{
    public class KorisnikService : IKorisnikService
    {
        public bool checkIfExist(string email)
        {
            Boolean value = false;
            using (var db = new DatabaseContext())
            {
                foreach (Korisnik k in db.Korisnici)
                {
                    if (k.Email.Equals(email))
                    {
                        value = true;
                        return value;
                    }
                }
            }
            return value;
        }
    }
}
