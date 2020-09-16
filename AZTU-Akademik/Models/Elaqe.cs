using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Elaqe
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }
        public string ScopusLink { get; set; }
        public string GoogleScholarLink { get; set; }
        public string Number { get; set; }
        public int? ArasdirmaciId { get; set; }

        public virtual Arasdirmacilar Arasdirmaci { get; set; }
    }
}
