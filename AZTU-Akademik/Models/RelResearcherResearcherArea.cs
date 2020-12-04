using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class RelResearcherResearcherArea
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? ResearcherId { get; set; }
        public int? AreaId { get; set; }

        public virtual ResearchArea Area { get; set; }
        public virtual User Researcher { get; set; }
    }
}
