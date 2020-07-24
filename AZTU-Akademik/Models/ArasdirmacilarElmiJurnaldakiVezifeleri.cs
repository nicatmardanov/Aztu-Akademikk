using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmacilarElmiJurnaldakiVezifeleri
    {
        public int Id { get; set; }
        public int? ArasdirmaciId { get; set; }
        public int? ElmiJurnaldakiVezifeId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual ElmiJurnaldakiVezifeler ElmiJurnaldakiVezife { get; set; }
    }
}
