using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class RelArticleResearcher
    {
        public int Id { get; set; }
        public int? ArticleId { get; set; }
        public int? IntAuthorId { get; set; }
        public int? ExtAuthorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public byte? Type { get; set; }

        public virtual Article Article { get; set; }
        public virtual ExternalResearcher ExtAuthor { get; set; }
        public virtual User IntAuthor { get; set; }
    }
}
