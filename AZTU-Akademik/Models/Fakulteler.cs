using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Fakulteler
    {
        public int Id { get; set; }
        public string FakulteAd { get; set; }
        public int? KafedraId { get; set; }

        public virtual Kafedralar Kafedra { get; set; }
    }
}
