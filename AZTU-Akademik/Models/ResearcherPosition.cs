using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ResearcherPosition
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? StatusId { get; set; }
        public int? ResearcherId { get; set; }
        public int? OrganizationId { get; set; }
        public int? PositionId { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual EducationOrganization Organization { get; set; }
        public virtual Position Position { get; set; }
        public virtual User Researcher { get; set; }
    }
}
