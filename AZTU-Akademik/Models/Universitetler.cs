using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Universitetler
    {
        public Universitetler()
        {
            Meqaleler = new HashSet<Meqaleler>();
        }

        public int Id { get; set; }
        public string UniversitetAd { get; set; }

        public virtual ICollection<Meqaleler> Meqaleler { get; set; }
    }
}
