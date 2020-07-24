using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class BakalavriatSiyahi
    {
        public BakalavriatSiyahi()
        {
            TehsilSeviyye = new HashSet<TehsilSeviyye>();
        }

        public int Id { get; set; }
        public int? BakalavrUniversitet { get; set; }
        public DateTime? BakalavrBaslangicIl { get; set; }
        public DateTime? BakalavrBitisIl { get; set; }
        public string BakalavrDisertasiyaAd { get; set; }
        public int? BakalavrDisertasiyaPdfId { get; set; }

        public virtual ICollection<TehsilSeviyye> TehsilSeviyye { get; set; }
    }
}
