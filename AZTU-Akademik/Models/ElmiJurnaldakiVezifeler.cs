using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ElmiJurnaldakiVezifeler
    {
        public ElmiJurnaldakiVezifeler()
        {
            ArasdirmacilarElmiJurnaldakiVezifeleri = new HashSet<ArasdirmacilarElmiJurnaldakiVezifeleri>();
        }

        public int Id { get; set; }
        public string VezifeAd { get; set; }

        public virtual ICollection<ArasdirmacilarElmiJurnaldakiVezifeleri> ArasdirmacilarElmiJurnaldakiVezifeleri { get; set; }
    }
}
