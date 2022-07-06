using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    
    public class LegendeNBALige
    {
        public int BrojGodina { get; set; }
        public String ImeIPrezime { get; set; }
        public DateTime Datum { get; set; }
        public String Slika { get; set; }
        public String Putanja { get; set; }

        public static HashSet<String> imena = new HashSet<string>();

        public LegendeNBALige()
        {
        
        }

        public LegendeNBALige(int brojGodina, String imeIPrezime, DateTime datum,String slika,String putanja)
        {
            this.BrojGodina = brojGodina;
            this.ImeIPrezime = imeIPrezime;
            this.Datum = datum;
            this.Slika = slika;
            this.Putanja = putanja;
        
        }

    }
}
