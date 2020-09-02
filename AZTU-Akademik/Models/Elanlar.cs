using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Elanlar
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Aciqlama { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
