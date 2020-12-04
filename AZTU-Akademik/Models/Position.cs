using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Position
    {
        public Position()
        {
            ResearcherPosition = new HashSet<ResearcherPosition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual ICollection<ResearcherPosition> ResearcherPosition { get; set; }
    }
}
