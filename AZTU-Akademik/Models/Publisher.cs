using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Textbook = new HashSet<Textbook>();
            Thesis = new HashSet<Thesis>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public virtual ICollection<Textbook> Textbook { get; set; }
        public virtual ICollection<Thesis> Thesis { get; set; }
    }
}
