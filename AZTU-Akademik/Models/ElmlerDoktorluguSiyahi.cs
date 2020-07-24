using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ElmlerDoktorluguSiyahi
    {
        public ElmlerDoktorluguSiyahi()
        {
            TehsilSeviyye = new HashSet<TehsilSeviyye>();
        }

        public int Id { get; set; }
        public int? ElmlerDoktoruUniversitetId { get; set; }
        public DateTime? ElmlerDoktoruBaslangicIl { get; set; }
        public DateTime? ElmlerDoktoruBitisIl { get; set; }
        public string ElmlerDoktoruDisertasiyaAd { get; set; }
        public int? ElmlerDoktoruDisertasiyaPdfId { get; set; }

        public virtual ICollection<TehsilSeviyye> TehsilSeviyye { get; set; }
    }
}
