using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaciDil
    {
        public int Id { get; set; }
        public int? XariciDilId { get; set; }
        public byte? DilSeviyye { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual DilSeviyye DilSeviyyeNavigation { get; set; }
        public virtual XariciDil XariciDil { get; set; }
    }
}
