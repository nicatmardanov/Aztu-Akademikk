using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Jurnallar
    {
        public Jurnallar()
        {
            Meqaleler = new HashSet<Meqaleler>();
        }

        public int Id { get; set; }
        public string JurnalAd { get; set; }

        public virtual ICollection<Meqaleler> Meqaleler { get; set; }
    }
}
