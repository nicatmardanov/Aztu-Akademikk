using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Arasdirmacilar
    {
        public Arasdirmacilar()
        {
            ArasdirmaSaheleri = new HashSet<ArasdirmaSaheleri>();
            ArasdirmaciMeqale = new HashSet<ArasdirmaciMeqale>();
            ArasdirmacilarElmiJurnaldakiVezifeleri = new HashSet<ArasdirmacilarElmiJurnaldakiVezifeleri>();
            IsTecrubesi = new HashSet<IsTecrubesi>();
            Mukafatlar = new HashSet<Mukafatlar>();
            Patentler = new HashSet<Patentler>();
            TehsilSeviyye = new HashSet<TehsilSeviyye>();
        }

        public int Id { get; set; }
        public string ArasdirmaciAd { get; set; }
        public string ArasdirmaciSoyad { get; set; }
        public int? KafedraId { get; set; }
        public int? TehsilSeviyyesiId { get; set; }
        public int? MeslekiIdariDeneyimID { get; set; }
        public string ArasdirmaciEmeil { get; set; }
        public string ArasdirmaciPassword { get; set; }
        public int? ArasdirmaciPedoqojiAdId { get; set; }
        public int? RolId { get; set; }

        public virtual ArasdirmaciPedoqojiAd ArasdirmaciPedoqojiAd { get; set; }
        public virtual Kafedralar Kafedra { get; set; }
        public virtual MeslekiIdariDeneyim MeslekiIdariDeneyim { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual ICollection<ArasdirmaSaheleri> ArasdirmaSaheleri { get; set; }
        public virtual ICollection<ArasdirmaciMeqale> ArasdirmaciMeqale { get; set; }
        public virtual ICollection<ArasdirmacilarElmiJurnaldakiVezifeleri> ArasdirmacilarElmiJurnaldakiVezifeleri { get; set; }
        public virtual ICollection<IsTecrubesi> IsTecrubesi { get; set; }
        public virtual ICollection<Mukafatlar> Mukafatlar { get; set; }
        public virtual ICollection<Patentler> Patentler { get; set; }
        public virtual ICollection<TehsilSeviyye> TehsilSeviyye { get; set; }
    }
}
