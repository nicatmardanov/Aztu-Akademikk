using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaciMeqale
    {
        public int Id { get; set; }
        public int? ArasdirmaciId { get; set; }
        public int? MeqaleId { get; set; }
        public bool? ElmiRehber { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual Meqaleler Meqale { get; set; }
    }
}
