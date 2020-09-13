using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Meqaleler
    {
        public Meqaleler()
        {
            ArasdirmaciMeqale = new HashSet<ArasdirmaciMeqale>();
            Pdfler = new HashSet<Pdfler>();
        }

        public int Id { get; set; }
        public string MeqaleAd { get; set; }
        public string MeqaleHaqqinda { get; set; }
        public DateTime? MeqaleIl { get; set; }
        public byte? MeqaleNovId { get; set; }
        public string Olke { get; set; }
        public int? UniversitetId { get; set; }
        public int? SaheId { get; set; }
        public int? MeqaleJurnalId { get; set; }
        public bool? IndeksMeqale { get; set; }

        public virtual Jurnallar MeqaleJurnal { get; set; }
        public virtual MeqaleNov MeqaleNov { get; set; }
        public virtual Universitetler Universitet { get; set; }
        public virtual ICollection<ArasdirmaciMeqale> ArasdirmaciMeqale { get; set; }
        public virtual ICollection<Pdfler> Pdfler { get; set; }
    }
}
