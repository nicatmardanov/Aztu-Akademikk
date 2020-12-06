using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ResearcherEducation
    {
        public ResearcherEducation()
        {
            Dissertation = new HashSet<Dissertation>();
        }

        public long Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? ResearcherId { get; set; }
        public short? FormId { get; set; }
        public short? LevelId { get; set; }
        public int? OrganizationId { get; set; }
        public short? CountryId { get; set; }
        public short? LanguageId { get; set; }
        public int? ProfessionId { get; set; }
        public byte? StatusId { get; set; }

        public virtual Country Country { get; set; }
        public virtual EducationForm Form { get; set; }
        public virtual Language Language { get; set; }
        public virtual EducationLevel Level { get; set; }
        public virtual EducationOrganization Organization { get; set; }
        public virtual Profession Profession { get; set; }
        public virtual User Researcher { get; set; }
        public virtual ICollection<Dissertation> Dissertation { get; set; }
    }
}
