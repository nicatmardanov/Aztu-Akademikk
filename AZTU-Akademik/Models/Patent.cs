using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Patent
    {
        public Patent()
        {
            RelPatentResearcher = new HashSet<RelPatentResearcher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? OrganizationId { get; set; }
        public int? ResearcherId { get; set; }

        public virtual EducationOrganization Organization { get; set; }
        public virtual User Researcher { get; set; }
        public virtual ICollection<RelPatentResearcher> RelPatentResearcher { get; set; }
    }
}
