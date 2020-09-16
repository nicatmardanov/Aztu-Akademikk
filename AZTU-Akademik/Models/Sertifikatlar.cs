using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Sertifikatlar
    {
        public int Id { get; set; }
        public string SertifikatAd { get; set; }
        public int? ArasdirmaciId { get; set; }
        public string Aciqlama { get; set; }
        public string SertifikatLink { get; set; }
        public string PdfAdres { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
