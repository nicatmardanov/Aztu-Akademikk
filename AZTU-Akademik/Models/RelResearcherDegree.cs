using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class RelResearcherDegree
    {
        public int Id { get; set; }
        public int? ResearcherId { get; set; }
        public int? DegreeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual EducationDegree Degree { get; set; }
        public virtual User Researcher { get; set; }
    }
}
