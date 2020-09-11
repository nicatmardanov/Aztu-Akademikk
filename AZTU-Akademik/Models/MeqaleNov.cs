using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class MeqaleNov
    {
        public MeqaleNov()
        {
            Meqaleler = new HashSet<Meqaleler>();
        }

        public byte Id { get; set; }
        public string MeqaleNovAd { get; set; }

        public virtual ICollection<Meqaleler> Meqaleler { get; set; }
    }
}
