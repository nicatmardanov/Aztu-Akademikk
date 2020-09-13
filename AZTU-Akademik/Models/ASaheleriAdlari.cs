using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ASaheleriAdlari
    {
        public ASaheleriAdlari()
        {
            ArasdirmaSaheleri = new HashSet<ArasdirmaSaheleri>();
        }

        public int Id { get; set; }
        public int? KafedraId { get; set; }
        public string Ad { get; set; }

        public virtual Kafedralar Kafedra { get; set; }
        public virtual ICollection<ArasdirmaSaheleri> ArasdirmaSaheleri { get; set; }
    }
}
