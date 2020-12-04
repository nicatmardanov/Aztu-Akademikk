using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ContactType
    {
        public ContactType()
        {
            Contact = new HashSet<Contact>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }
    }
}
