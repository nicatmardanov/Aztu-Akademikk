using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class Country
    {
        public Country()
        {
            ResearcherEducation = new HashSet<ResearcherEducation>();
            UserCitizenship = new HashSet<User>();
            UserNationality = new HashSet<User>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }

        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual ICollection<User> UserCitizenship { get; set; }
        public virtual ICollection<User> UserNationality { get; set; }
    }
}
