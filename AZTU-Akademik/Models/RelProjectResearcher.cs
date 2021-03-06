﻿using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class RelProjectResearcher
    {
        public long Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool? Type { get; set; }
        public int? ProjectId { get; set; }
        public int? IntAuthorId { get; set; }
        public int? ExtAuthorId { get; set; }
        public byte? StatusId { get; set; }

        public virtual ExternalResearcher ExtAuthor { get; set; }
        public virtual User IntAuthor { get; set; }
        public virtual Project Project { get; set; }
    }
}
