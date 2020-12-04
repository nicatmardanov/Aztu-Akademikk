using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public short? TypeId { get; set; }
        public int? ResearcherId { get; set; }

        public virtual User Researcher { get; set; }
        public virtual ContactType Type { get; set; }
    }
}
