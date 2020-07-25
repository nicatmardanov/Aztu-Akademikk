using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class XariciDil
    {
        public XariciDil()
        {
            ArasdirmaciDil = new HashSet<ArasdirmaciDil>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }

        public virtual ICollection<ArasdirmaciDil> ArasdirmaciDil { get; set; }
    }
}
