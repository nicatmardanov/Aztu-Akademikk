using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class MagistranturaSiyahisi
    {
        public MagistranturaSiyahisi()
        {
            TehsilSeviyye = new HashSet<TehsilSeviyye>();
        }

        public int Id { get; set; }
        public int? MagistrUniversitetId { get; set; }
        public DateTime? MagistrBaslangicIl { get; set; }
        public DateTime? MagistrBitisIl { get; set; }
        public string MagistrDisertasiyaAd { get; set; }
        public int? MagistrDisertasiyaPdfId { get; set; }

        public virtual ICollection<TehsilSeviyye> TehsilSeviyye { get; set; }
    }
}
