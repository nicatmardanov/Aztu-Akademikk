using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaciDil
    {
        public int Id { get; set; }
        public int? XariciDilId { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual XariciDil XariciDil { get; set; }
    }
}
