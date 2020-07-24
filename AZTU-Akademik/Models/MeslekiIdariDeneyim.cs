using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class MeslekiIdariDeneyim
    {
        public MeslekiIdariDeneyim()
        {
            Arasdirmacilar = new HashSet<Arasdirmacilar>();
        }

        public int Id { get; set; }
        public string MeslekiIdariDeneyimAd { get; set; }

        public virtual ICollection<Arasdirmacilar> Arasdirmacilar { get; set; }
    }
}
