using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Model
{
    public class Dogadjaj
    {
        public enum STATUS_DOGADJAJA
        {
            NEDODELJEN,
            CEKA_SE_ORGANIZATOR,
            CEKA_SE_KLIJENT,
            ORGANIZOVAN
        }

        public int Id { get; set; }

        public double Budzet { get; set; }

        public int BrojGostiju { get; set; }

        public List<Gost> NerasporedjeniGosti { get; set; }

        public List<KapacitetStola> RasporedSedenja { get; set; }

        public DateTime DatumOdrzavanja { get; set; }

        public string VrstaProslave { get; set; }

        public string Tema { get; set; }

        public string Napomena { get; set; }

        private STATUS_DOGADJAJA _statusEnum;
        public STATUS_DOGADJAJA StatusEnum
        {
            get
            {
                return _statusEnum;
            }
            set
            {
                _statusEnum = value;
                if (_statusEnum == STATUS_DOGADJAJA.NEDODELJEN)
                {
                    _status = "Nema dodeljenog organizatora";
                }
                else if (_statusEnum == STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR)
                {
                    _status = "Čeka se odgovor organizatora";
                }
                else if (_statusEnum == STATUS_DOGADJAJA.CEKA_SE_KLIJENT)
                {
                    _status = "Čeka se odgovor klijenta";
                }
                else if (_statusEnum == STATUS_DOGADJAJA.ORGANIZOVAN)
                {
                    _status = "Organizovan";
                }
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if (_status.Equals("Nema dodeljenog organizatora"))
                {
                    _statusEnum = STATUS_DOGADJAJA.NEDODELJEN;
                }
                else if (_status.Equals("Čeka se odgovor organizatora"))
                {
                    _statusEnum = STATUS_DOGADJAJA.CEKA_SE_ORGANIZATOR;
                }
                else if (_status.Equals("Čeka se odgovor klijenta"))
                {
                    _statusEnum = STATUS_DOGADJAJA.CEKA_SE_KLIJENT;
                }
                else if (_status.Equals("Organizovan"))
                {
                    _statusEnum = STATUS_DOGADJAJA.ORGANIZOVAN;
                }
            }
        }

        public bool BudzetPromenljiv { get; set; }

        public List<Zadatak> Zadaci { get; set; }

        public string DodatniZahtevi { get; set; }

        public Organizator Organizator { get; set; }

        public Predlog PrihvaceniGlavniPredlog { get; set; }

        public List<Predlog> PrihvaceniDodatniPredlozi { get; set; }

        public string MestoOdrzavanja { get; set; }

        public List<Komentar> Komentari { get; set; }
    }
}
