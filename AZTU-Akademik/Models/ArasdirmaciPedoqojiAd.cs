using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaciPedoqojiAd
    {
        public ArasdirmaciPedoqojiAd()
        {
            Arasdirmacilar = new HashSet<Arasdirmacilar>();
        }

        public int ArasdirmaciPedoqojiAdId { get; set; }
        public string ArasdirmaciAd { get; set; }

        public virtual ICollection<Arasdirmacilar> Arasdirmacilar { get; set; }
    }
}
