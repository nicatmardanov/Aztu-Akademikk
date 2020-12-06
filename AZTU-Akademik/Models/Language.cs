using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Language
    {
        public Language()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
            ResearcherLanguage = new HashSet<ResearcherLanguage>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Iso6391 { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual ICollection<ResearcherLanguage> ResearcherLanguage { get; set; }
    }
}
