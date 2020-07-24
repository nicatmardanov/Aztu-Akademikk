using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ElmlerNamizedlikSiyahi
    {
        public ElmlerNamizedlikSiyahi()
        {
            TehsilSeviyye = new HashSet<TehsilSeviyye>();
        }

        public int Id { get; set; }
        public int? ElmlerNamizediUniversitetId { get; set; }
        public DateTime? ElmlerNamizediBaslangicIl { get; set; }
        public DateTime? ElmlerNamizediBitisIl { get; set; }
        public string ElmlerNamizediDisertasiyaAd { get; set; }
        public int? ElmlerNamizediDisertasiyaPdfId { get; set; }

        public virtual ICollection<TehsilSeviyye> TehsilSeviyye { get; set; }
    }
}
