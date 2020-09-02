using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class AkademikIsTecrubesi
    {
        public int Id { get; set; }
        public DateTime? BaslangicIl { get; set; }
        public DateTime? SonIl { get; set; }
        public string IsYeri { get; set; }
        public int? ArasdirmaciId { get; set; }
        public short? AkademikTecrubeAdiId { get; set; }

        public virtual AkademikTecrubeAdlari AkademikTecrubeAdi { get; set; }
        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
