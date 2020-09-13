using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaSaheleri
    {
        public int Id { get; set; }
        public int? SaheId { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual ASaheleriAdlari Sahe { get; set; }
    }
}
