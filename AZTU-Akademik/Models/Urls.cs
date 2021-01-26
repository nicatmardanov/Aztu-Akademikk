using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Urls
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string UrlType { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? TextbookId { get; set; }
        public int? ThesisId { get; set; }
        public int? ArticleId { get; set; }
        public byte? StatusId { get; set; }

        public virtual Article Article { get; set; }
        public virtual Textbook Textbook { get; set; }
        public virtual Thesis Thesis { get; set; }
    }
}
