using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Department
    {
        public Department()
        {
            Profession = new HashSet<Profession>();
            ResearcherPosition = new HashSet<ResearcherPosition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Profession> Profession { get; set; }
        public virtual ICollection<ResearcherPosition> ResearcherPosition { get; set; }
    }
}
