using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ArasdirmaciAdministrativVezife
    {
        public int ArasdirmaciId { get; set; }
        public int AdministrativVezifeId { get; set; }

        public virtual AdministrativVezifeler AdministrativVezife { get; set; }
        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
