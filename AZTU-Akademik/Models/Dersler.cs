using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Dersler
    {
        public int Id { get; set; }
        public int? KafedraId { get; set; }
        public string DersSeviyye { get; set; }

        public virtual Kafedralar Kafedra { get; set; }
    }
}
