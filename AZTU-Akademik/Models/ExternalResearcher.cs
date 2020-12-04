using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ExternalResearcher
    {
        public ExternalResearcher()
        {
            RelArticleResearcher = new HashSet<RelArticleResearcher>();
            RelPatentResearcher = new HashSet<RelPatentResearcher>();
            RelProjectResearcher = new HashSet<RelProjectResearcher>();
            RelTextbookResearcher = new HashSet<RelTextbookResearcher>();
            RelThesisResearcher = new HashSet<RelThesisResearcher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? OrganizationId { get; set; }

        public virtual EducationOrganization Organization { get; set; }
        public virtual ICollection<RelArticleResearcher> RelArticleResearcher { get; set; }
        public virtual ICollection<RelPatentResearcher> RelPatentResearcher { get; set; }
        public virtual ICollection<RelProjectResearcher> RelProjectResearcher { get; set; }
        public virtual ICollection<RelTextbookResearcher> RelTextbookResearcher { get; set; }
        public virtual ICollection<RelThesisResearcher> RelThesisResearcher { get; set; }
    }
}
