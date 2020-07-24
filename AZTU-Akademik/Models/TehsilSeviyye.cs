using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class TehsilSeviyye
    {
        public int Id { get; set; }
        public int? BakalavrId { get; set; }
        public int? MagistrId { get; set; }
        public int? ElmlerNamizediId { get; set; }
        public int? ElmlerDoktoru { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
        public virtual BakalavriatSiyahi Bakalavr { get; set; }
        public virtual ElmlerDoktorluguSiyahi ElmlerDoktoruNavigation { get; set; }
        public virtual ElmlerNamizedlikSiyahi ElmlerNamizedi { get; set; }
        public virtual MagistranturaSiyahisi Magistr { get; set; }
    }
}
