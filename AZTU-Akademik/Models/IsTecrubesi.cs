using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class IsTecrubesi
    {
        public int Id { get; set; }
        public string IsVezife { get; set; }
        public string IsYeri { get; set; }
        public DateTime? IsBaslangicTarixi { get; set; }
        public DateTime? IsBitisTarixi { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
