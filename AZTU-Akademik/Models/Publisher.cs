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

        public virtual ICollection<Textbook> Textbook { get; set; }
        public virtual ICollection<Thesis> Thesis { get; set; }
    }
}
