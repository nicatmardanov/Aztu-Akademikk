using System;
using System.Collections.Generic;

namespace AZTU_Akademik.Models
{
    public partial class User
    {
        public User()
        {
            Announcement = new HashSet<Announcement>();
            Article = new HashSet<Article>();
            Certificate = new HashSet<Certificate>();
            Contact = new HashSet<Contact>();
            File = new HashSet<File>();
            Log = new HashSet<Log>();
            ManagementExperience = new HashSet<ManagementExperience>();
            PasswordReset = new HashSet<PasswordReset>();
            Patent = new HashSet<Patent>();
            Project = new HashSet<Project>();
            RelArticleResearcher = new HashSet<RelArticleResearcher>();
            RelPatentResearcher = new HashSet<RelPatentResearcher>();
            RelProjectResearcher = new HashSet<RelProjectResearcher>();
            RelResearcherDegree = new HashSet<RelResearcherDegree>();
            RelResearcherResearcherArea = new HashSet<RelResearcherResearcherArea>();
            RelTextbookResearcher = new HashSet<RelTextbookResearcher>();
            RelThesisResearcher = new HashSet<RelThesisResearcher>();
            ResearcherEducation = new HashSet<ResearcherEducation>();
            ResearcherLanguage = new HashSet<ResearcherLanguage>();
            ResearcherPosition = new HashSet<ResearcherPosition>();
            TextbookCreator = new HashSet<Textbook>();
            TextbookPublisher = new HashSet<Textbook>();
            ThesisCreator = new HashSet<Thesis>();
            ThesisPublisher = new HashSet<Thesis>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string ImageAddress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public byte? StatusId { get; set; }
        public short? NationalityId { get; set; }
        public short? CitizenshipId { get; set; }
        public byte? RoleId { get; set; }

        public virtual Country Citizenship { get; set; }
        public virtual Country Nationality { get; set; }
        public virtual ICollection<Announcement> Announcement { get; set; }
        public virtual ICollection<Article> Article { get; set; }
        public virtual ICollection<Certificate> Certificate { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<File> File { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<ManagementExperience> ManagementExperience { get; set; }
        public virtual ICollection<PasswordReset> PasswordReset { get; set; }
        public virtual ICollection<Patent> Patent { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<RelArticleResearcher> RelArticleResearcher { get; set; }
        public virtual ICollection<RelPatentResearcher> RelPatentResearcher { get; set; }
        public virtual ICollection<RelProjectResearcher> RelProjectResearcher { get; set; }
        public virtual ICollection<RelResearcherDegree> RelResearcherDegree { get; set; }
        public virtual ICollection<RelResearcherResearcherArea> RelResearcherResearcherArea { get; set; }
        public virtual ICollection<RelTextbookResearcher> RelTextbookResearcher { get; set; }
        public virtual ICollection<RelThesisResearcher> RelThesisResearcher { get; set; }
        public virtual ICollection<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual ICollection<ResearcherLanguage> ResearcherLanguage { get; set; }
        public virtual ICollection<ResearcherPosition> ResearcherPosition { get; set; }
        public virtual ICollection<Textbook> TextbookCreator { get; set; }
        public virtual ICollection<Textbook> TextbookPublisher { get; set; }
        public virtual ICollection<Thesis> ThesisCreator { get; set; }
        public virtual ICollection<Thesis> ThesisPublisher { get; set; }
    }
}
