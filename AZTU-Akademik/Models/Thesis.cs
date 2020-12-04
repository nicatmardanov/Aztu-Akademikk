﻿using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Thesis
    {
        public Thesis()
        {
            RelThesisResearcher = new HashSet<RelThesisResearcher>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public int? PublisherId { get; set; }
        public int? CreatorId { get; set; }

        public virtual User Creator { get; set; }
        public virtual User Publisher { get; set; }
        public virtual ICollection<RelThesisResearcher> RelThesisResearcher { get; set; }
    }
}
