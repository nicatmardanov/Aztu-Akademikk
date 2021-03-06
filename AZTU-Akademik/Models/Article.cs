﻿using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Article
    {
        public Article()
        {
            RelArticleResearcher = new HashSet<RelArticleResearcher>();
            Urls = new HashSet<Urls>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public string Volume { get; set; }
        public int? PageStart { get; set; }
        public int? PageEnd { get; set; }
        public string Url { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? CreatorId { get; set; }
        public long? FileId { get; set; }
        public int? Journal { get; set; }

        public virtual User Creator { get; set; }
        public virtual File File { get; set; }
        public virtual Journal JournalNavigation { get; set; }
        public virtual ICollection<RelArticleResearcher> RelArticleResearcher { get; set; }
        public virtual ICollection<Urls> Urls { get; set; }
    }
}
