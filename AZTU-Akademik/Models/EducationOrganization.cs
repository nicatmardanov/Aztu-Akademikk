using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class EducationOrganization
    {
        public EducationOrganization()
        {
            ExternalResearcher = new HashSet<ExternalResearcher>();
            ManagementExperience = new HashSet<ManagementExperience>();
            Patent = new HashSet<Patent>();
            Project = new HashSet<Project>();
            ResearcherEducation = new HashSet<ResearcherEducation>();
            ResearcherPosition = new HashSet<ResearcherPosition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public byte? TypeId { get; set; }

        public virtual EducationOrganizationType Type { get; set; }
        public virtual ICollection<ExternalResearcher> ExternalResearcher { get; set; }
        public virtual ICollection<ManagementExperience> ManagementExperience { get; set; }
        public virtual ICollection<Patent> Patent { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual ICollection<ResearcherPosition> ResearcherPosition { get; set; }
    }
}
