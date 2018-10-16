using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SedniceNNV.Models
{
    public class ApiModel
    {
        public partial class ClanoviApi
        {
            
            public int ClanID { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            public string KorisnickoIme { get; set; }
            public string Lozinka { get; set; }
            public string Uloga { get; set; }

           
        }

        public partial class SedniceApi
        {
            public int SednicaID { get; set; }
            public System.DateTime Datum { get; set; }
            public string Vrsta { get; set; }
            public string Ucionica { get; set; }
            public string Zapisnik { get; set; }
            public string Poziv { get; set; }
        }
    }
}