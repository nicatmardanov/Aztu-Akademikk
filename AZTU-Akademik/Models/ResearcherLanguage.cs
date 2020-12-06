using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class ResearcherLanguage
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? ResearcherId { get; set; }
        public short? LanguageId { get; set; }
        public short? LevelId { get; set; }
        public long? FileId { get; set; }

        public virtual File File { get; set; }
        public virtual Language Language { get; set; }
        public virtual LanguageLevels Level { get; set; }
        public virtual User Researcher { get; set; }
    }
}
