using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class DersArasdirmaci
    {
        public int? DersId { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual Dersler Ders { get; set; }
    }
}
