﻿using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class File
    {
        public File()
        {
            Article = new HashSet<Article>();
            Certificate = new HashSet<Certificate>();
            Dissertation = new HashSet<Dissertation>();
            ResearcherLanguage = new HashSet<ResearcherLanguage>();
            Textbook = new HashSet<Textbook>();
            Thesis = new HashSet<Thesis>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Article> Article { get; set; }
        public virtual ICollection<Certificate> Certificate { get; set; }
        public virtual ICollection<Dissertation> Dissertation { get; set; }
        public virtual ICollection<ResearcherLanguage> ResearcherLanguage { get; set; }
        public virtual ICollection<Textbook> Textbook { get; set; }
        public virtual ICollection<Thesis> Thesis { get; set; }
    }
}
