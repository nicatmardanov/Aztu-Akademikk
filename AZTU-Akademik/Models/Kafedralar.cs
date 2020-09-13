using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Kafedralar
    {
        public Kafedralar()
        {
            ASaheleriAdlari = new HashSet<ASaheleriAdlari>();
            Arasdirmacilar = new HashSet<Arasdirmacilar>();
            Dersler = new HashSet<Dersler>();
            Fakulteler = new HashSet<Fakulteler>();
        }

        public int Id { get; set; }
        public string KafedraAd { get; set; }

        public virtual ICollection<ASaheleriAdlari> ASaheleriAdlari { get; set; }
        public virtual ICollection<Arasdirmacilar> Arasdirmacilar { get; set; }
        public virtual ICollection<Dersler> Dersler { get; set; }
        public virtual ICollection<Fakulteler> Fakulteler { get; set; }
    }
}
