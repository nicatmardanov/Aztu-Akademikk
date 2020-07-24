using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Mukafatlar
    {
        public int Id { get; set; }
        public int ArasdirmaciId { get; set; }
        public string MukafatAd { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
