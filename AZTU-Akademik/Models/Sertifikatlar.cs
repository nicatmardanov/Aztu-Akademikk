using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Sertifikatlar
    {
        public int ArasdirmaciId { get; set; }
        public string SertifikatAd { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
