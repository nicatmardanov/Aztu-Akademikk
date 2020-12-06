using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Dissertation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public long? EducationId { get; set; }
        public long? FileId { get; set; }
        public byte? StatusId { get; set; }

        public virtual ResearcherEducation Education { get; set; }
        public virtual File File { get; set; }
    }
}
