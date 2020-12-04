using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Profession
    {
        public Profession()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
    }
}
