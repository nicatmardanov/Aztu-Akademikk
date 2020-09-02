using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class AkademikTecrubeAdlari
    {
        public AkademikTecrubeAdlari()
        {
            AkademikIsTecrubesi = new HashSet<AkademikIsTecrubesi>();
        }

        public short Id { get; set; }
        public string TecrubeAdi { get; set; }

        public virtual ICollection<AkademikIsTecrubesi> AkademikIsTecrubesi { get; set; }
    }
}
