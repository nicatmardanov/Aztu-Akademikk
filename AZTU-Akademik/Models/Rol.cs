using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Arasdirmacilar = new HashSet<Arasdirmacilar>();
        }

        public int Id { get; set; }
        public string RolAd { get; set; }

        public virtual ICollection<Arasdirmacilar> Arasdirmacilar { get; set; }
    }
}
