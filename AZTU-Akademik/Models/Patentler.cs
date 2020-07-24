using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Patentler
    {
        public int Id { get; set; }
        public string PatentAd { get; set; }
        public int ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
