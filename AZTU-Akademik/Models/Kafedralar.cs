using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Kafedralar
    {
        public Kafedralar()
        {
            ArasdirmaSaheleri = new HashSet<ArasdirmaSaheleri>();
            Arasdirmacilar = new HashSet<Arasdirmacilar>();
            Dersler = new HashSet<Dersler>();
            Fakulteler = new HashSet<Fakulteler>();
        }

        public int Id { get; set; }
        public string KafedraAd { get; set; }

        public virtual ICollection<ArasdirmaSaheleri> ArasdirmaSaheleri { get; set; }
        public virtual ICollection<Arasdirmacilar> Arasdirmacilar { get; set; }
        public virtual ICollection<Dersler> Dersler { get; set; }
        public virtual ICollection<Fakulteler> Fakulteler { get; set; }
    }
}
