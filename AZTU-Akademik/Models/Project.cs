using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Project
    {
        public Project()
        {
            RelProjectResearcher = new HashSet<RelProjectResearcher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? ResearcherId { get; set; }

        public virtual EducationOrganization Organization { get; set; }
        public virtual User Researcher { get; set; }
        public virtual ICollection<RelProjectResearcher> RelProjectResearcher { get; set; }
    }
}
