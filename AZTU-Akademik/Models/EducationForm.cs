using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class EducationForm
    {
        public EducationForm()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
    }
}
