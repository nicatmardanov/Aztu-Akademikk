using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ManagementExperience
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ResearcherId { get; set; }
        public int? OrganizationId { get; set; }

        public virtual EducationOrganization Organization { get; set; }
        public virtual User Researcher { get; set; }
    }
}
