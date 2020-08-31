using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class DilSeviyye
    {
        public DilSeviyye()
        {
            ArasdirmaciDil = new HashSet<ArasdirmaciDil>();
        }

        public byte Id { get; set; }
        public string SeviyyeAd { get; set; }

        public virtual ICollection<ArasdirmaciDil> ArasdirmaciDil { get; set; }
    }
}
