﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AZTU_Akademik.Models
{
    public partial class AztuAkademikContext : DbContext
    {
        public AztuAkademikContext()
        {
        }

        public AztuAkademikContext(DbContextOptions<AztuAkademikContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Dissertation> Dissertation { get; set; }
        public virtual DbSet<EducationDegree> EducationDegree { get; set; }
        public virtual DbSet<EducationForm> EducationForm { get; set; }
        public virtual DbSet<EducationLevel> EducationLevel { get; set; }
        public virtual DbSet<EducationOrganization> EducationOrganization { get; set; }
        public virtual DbSet<EducationOrganizationType> EducationOrganizationType { get; set; }
        public virtual DbSet<ExternalResearcher> ExternalResearcher { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<Journal> Journal { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LanguageLevels> LanguageLevels { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<ManagementExperience> ManagementExperience { get; set; }
        public virtual DbSet<Operation> Operation { get; set; }
        public virtual DbSet<PasswordReset> PasswordReset { get; set; }
        public virtual DbSet<Patent> Patent { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Profession> Profession { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<RelArticleResearcher> RelArticleResearcher { get; set; }
        public virtual DbSet<RelPatentResearcher> RelPatentResearcher { get; set; }
        public virtual DbSet<RelProjectResearcher> RelProjectResearcher { get; set; }
        public virtual DbSet<RelResearcherDegree> RelResearcherDegree { get; set; }
        public virtual DbSet<RelResearcherResearcherArea> RelResearcherResearcherArea { get; set; }
        public virtual DbSet<RelTextbookResearcher> RelTextbookResearcher { get; set; }
        public virtual DbSet<RelThesisResearcher> RelThesisResearcher { get; set; }
        public virtual DbSet<ResearchArea> ResearchArea { get; set; }
        public virtual DbSet<ResearcherEducation> ResearcherEducation { get; set; }
        public virtual DbSet<ResearcherLanguage> ResearcherLanguage { get; set; }
        public virtual DbSet<ResearcherPosition> ResearcherPosition { get; set; }
        public virtual DbSet<Textbook> Textbook { get; set; }
        public virtual DbSet<Thesis> Thesis { get; set; }
        public virtual DbSet<Urls> Urls { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-UTUBGGC\\SQLEXPRESS;Database=Aztu-Akademik;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Announcement_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(500);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.Announcement)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Announcement_User");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Article_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Journal).HasColumnName("journal");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PageEnd).HasColumnName("page_end");

                entity.Property(e => e.PageStart).HasColumnName("page_start");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Article_User");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_Article_File");

                entity.HasOne(d => d.JournalNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Journal)
                    .HasConstraintName("FK_Article_Journal");
            });

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Certificate_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Certificate)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_Certificate_File");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.Certificate)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Certificate_User");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Contact_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.Contact)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Contact_User");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Contact)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Contact_ContactType");
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ContactType_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Icon).HasColumnName("icon");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Country_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Department_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(100);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Department)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK_Department_Faculty");
            });

            modelBuilder.Entity<Dissertation>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Dissertation_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EducationId).HasColumnName("education_id");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Education)
                    .WithMany(p => p.Dissertation)
                    .HasForeignKey(d => d.EducationId)
                    .HasConstraintName("FK_Dissertation_ResearcherEducation");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Dissertation)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_Dissertation_File");
            });

            modelBuilder.Entity<EducationDegree>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_EducationDegree_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EducationForm>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_EducationForm_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EducationLevel>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_EducationLevel_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EducationOrganization>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_EducationOrganization_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(100);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.EducationOrganization)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_EducationOrganization_EducationOrganizationType");
            });

            modelBuilder.Entity<EducationOrganizationType>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_EducationOrganizationType_DeleteDate");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ExternalResearcher>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ExternalResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ExternalResearcher)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ExternalResearcher_EducationOrganization");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Faculty_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_File_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.File)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_File_User");
            });

            modelBuilder.Entity<Journal>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Journal_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Indexed).HasColumnName("indexed");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Language_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Iso6391)
                    .HasColumnName("iso_639_1")
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<LanguageLevels>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_LanguageLevels_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(500);

                entity.Property(e => e.ShortName)
                    .HasColumnName("short_name")
                    .HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasIndex(e => e.TableName)
                    .HasName("IX_Log_TableName");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Log_UserId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInformation)
                    .HasColumnName("additional_information")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ipAddress")
                    .HasMaxLength(300);

                entity.Property(e => e.OperationId).HasColumnName("operation_id");

                entity.Property(e => e.RefId).HasColumnName("ref_id");

                entity.Property(e => e.TableName)
                    .HasColumnName("table_name")
                    .HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Log)
                    .HasForeignKey(d => d.OperationId)
                    .HasConstraintName("FK_Log_Operation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Log)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Log_User");
            });

            modelBuilder.Entity<ManagementExperience>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ManagementExperience_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ManagementExperience)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ManagementExperience_EducationOrganization");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ManagementExperience)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_ManagementExperience_User");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_PasswordReset_DeleteDate");

                entity.HasIndex(e => e.Hash)
                    .HasName("IX_PasswordReset_Hash");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_PasswordReset_UserId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hash)
                    .HasColumnName("hash")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordReset)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PasswordReset_User");
            });

            modelBuilder.Entity<Patent>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Patent_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplyDate)
                    .HasColumnName("apply_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnName("registration_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Patent)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Patent_EducationOrganization");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.Patent)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Patent_User");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Position_DeleteDate");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Profession>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Profession_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Profession)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Profession_Department");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Project_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Project_EducationOrganization");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Project_User");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Publisher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(700);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<RelArticleResearcher>(entity =>
            {
                entity.ToTable("Rel_ArticleResearcher");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_ArticleResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArticleId).HasColumnName("article_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtAuthorId).HasColumnName("ext_author_id");

                entity.Property(e => e.IntAuthorId).HasColumnName("int_author_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.RelArticleResearcher)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_Rel_ArticleResearcher_Article");

                entity.HasOne(d => d.ExtAuthor)
                    .WithMany(p => p.RelArticleResearcher)
                    .HasForeignKey(d => d.ExtAuthorId)
                    .HasConstraintName("FK_Rel_ArticleResearcher_ExternalResearcher");

                entity.HasOne(d => d.IntAuthor)
                    .WithMany(p => p.RelArticleResearcher)
                    .HasForeignKey(d => d.IntAuthorId)
                    .HasConstraintName("FK_Rel_ArticleResearcher_User");
            });

            modelBuilder.Entity<RelPatentResearcher>(entity =>
            {
                entity.ToTable("Rel_PatentResearcher");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_PatentResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtAuthorId).HasColumnName("ext_author_id");

                entity.Property(e => e.IntAuthorId).HasColumnName("int_author_id");

                entity.Property(e => e.PatentId).HasColumnName("patent_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ExtAuthor)
                    .WithMany(p => p.RelPatentResearcher)
                    .HasForeignKey(d => d.ExtAuthorId)
                    .HasConstraintName("FK_Rel_PatentResearcher_ExternalResearcher");

                entity.HasOne(d => d.IntAuthor)
                    .WithMany(p => p.RelPatentResearcher)
                    .HasForeignKey(d => d.IntAuthorId)
                    .HasConstraintName("FK_Rel_PatentResearcher_User");

                entity.HasOne(d => d.Patent)
                    .WithMany(p => p.RelPatentResearcher)
                    .HasForeignKey(d => d.PatentId)
                    .HasConstraintName("FK_Rel_PatentResearcher_Patent");
            });

            modelBuilder.Entity<RelProjectResearcher>(entity =>
            {
                entity.ToTable("Rel_ProjectResearcher");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_ProjectResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtAuthorId).HasColumnName("ext_author_id");

                entity.Property(e => e.IntAuthorId).HasColumnName("int_author_id");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ExtAuthor)
                    .WithMany(p => p.RelProjectResearcher)
                    .HasForeignKey(d => d.ExtAuthorId)
                    .HasConstraintName("FK_Rel_ProjectResearcher_ExternalResearcher");

                entity.HasOne(d => d.IntAuthor)
                    .WithMany(p => p.RelProjectResearcher)
                    .HasForeignKey(d => d.IntAuthorId)
                    .HasConstraintName("FK_Rel_ProjectResearcher_User");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.RelProjectResearcher)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Rel_ProjectResearcher_Project");
            });

            modelBuilder.Entity<RelResearcherDegree>(entity =>
            {
                entity.ToTable("Rel_ResearcherDegree");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_ResearcherDegree_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DegreeId).HasColumnName("degree_id");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Degree)
                    .WithMany(p => p.RelResearcherDegree)
                    .HasForeignKey(d => d.DegreeId)
                    .HasConstraintName("FK_Rel_ResearcherDegree_EducationDegree");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.RelResearcherDegree)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Rel_ResearcherDegree_User");
            });

            modelBuilder.Entity<RelResearcherResearcherArea>(entity =>
            {
                entity.ToTable("Rel_Researcher_ResearcherArea");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_Researcher_ResearcherArea_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AreaId).HasColumnName("area_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.RelResearcherResearcherArea)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Rel_Researcher_ResearcherArea_ResearchArea");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.RelResearcherResearcherArea)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_Rel_Researcher_ResearcherArea_User");
            });

            modelBuilder.Entity<RelTextbookResearcher>(entity =>
            {
                entity.ToTable("Rel_TextbookResearcher");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_TextbookResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtAuthorId).HasColumnName("ext_author_id");

                entity.Property(e => e.IntAuthorId).HasColumnName("int_author_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TextbookId).HasColumnName("textbook_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ExtAuthor)
                    .WithMany(p => p.RelTextbookResearcher)
                    .HasForeignKey(d => d.ExtAuthorId)
                    .HasConstraintName("FK_Rel_TextbookResearcher_ExternalResearcher");

                entity.HasOne(d => d.IntAuthor)
                    .WithMany(p => p.RelTextbookResearcher)
                    .HasForeignKey(d => d.IntAuthorId)
                    .HasConstraintName("FK_Rel_TextbookResearcher_User");

                entity.HasOne(d => d.Textbook)
                    .WithMany(p => p.RelTextbookResearcher)
                    .HasForeignKey(d => d.TextbookId)
                    .HasConstraintName("FK_Rel_TextbookResearcher_Textbook");
            });

            modelBuilder.Entity<RelThesisResearcher>(entity =>
            {
                entity.ToTable("Rel_ThesisResearcher");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Rel_ThesisResearcher_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtAuthorId).HasColumnName("ext_author_id");

                entity.Property(e => e.IntAuthorId).HasColumnName("int_author_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.ThesisId).HasColumnName("thesis_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ExtAuthor)
                    .WithMany(p => p.RelThesisResearcher)
                    .HasForeignKey(d => d.ExtAuthorId)
                    .HasConstraintName("FK_Rel_ThesisResearcher_ExternalResearcher");

                entity.HasOne(d => d.IntAuthor)
                    .WithMany(p => p.RelThesisResearcher)
                    .HasForeignKey(d => d.IntAuthorId)
                    .HasConstraintName("FK_Rel_ThesisResearcher_User");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.RelThesisResearcher)
                    .HasForeignKey(d => d.ThesisId)
                    .HasConstraintName("FK_Rel_ThesisResearcher_Thesis");
            });

            modelBuilder.Entity<ResearchArea>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ResearchArea_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ResearcherEducation>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ResearcherEducation_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FormId).HasColumnName("form_id");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.LevelId).HasColumnName("level_id");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.ProfessionId).HasColumnName("profession_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_ResearcherEducation_Country");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_ResearcherEducation_EducationForm");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_ResearcherEducation_Language");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_ResearcherEducation_EducationLevel");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ResearcherEducation_EducationOrganization");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.ProfessionId)
                    .HasConstraintName("FK_ResearcherEducation_Profession");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherEducation)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_ResearcherEducation_User");
            });

            modelBuilder.Entity<ResearcherLanguage>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ResearcherLanguage_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.LevelId).HasColumnName("level_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.ResearcherLanguage)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_ResearcherLanguage_File");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.ResearcherLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_ResearcherLanguage_Language");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.ResearcherLanguage)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK_ResearcherLanguage_LanguageLevels");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherLanguage)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_ResearcherLanguage_User");
            });

            modelBuilder.Entity<ResearcherPosition>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_ResearcherPosition_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrganizationId).HasColumnName("organization_id");

                entity.Property(e => e.PositionId).HasColumnName("position_id");

                entity.Property(e => e.ResearcherId).HasColumnName("researcher_id");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.ResearcherPosition)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_ResearcherPosition_Department");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ResearcherPosition)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_ResearcherPosition_EducationOrganization");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.ResearcherPosition)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_ResearcherPosition_Position");

                entity.HasOne(d => d.Researcher)
                    .WithMany(p => p.ResearcherPosition)
                    .HasForeignKey(d => d.ResearcherId)
                    .HasConstraintName("FK_ResearcherPosition_User");
            });

            modelBuilder.Entity<Textbook>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Textbook_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Textbook)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Textbook_User2");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Textbook)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_Textbook_File");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Textbook)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Textbook_Publisher");
            });

            modelBuilder.Entity<Thesis>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Thesis_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FileId).HasColumnName("file_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Thesis)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Thesis_User2");

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Thesis)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK_Thesis_File");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Thesis)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Thesis_Publisher");
            });

            modelBuilder.Entity<Urls>(entity =>
            {
                entity.HasIndex(e => e.ArticleId)
                    .HasName("IX_Urls_Article");

                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_Urls_DeleteDate");

                entity.HasIndex(e => e.TextbookId)
                    .HasName("IX_Urls_Textbook");

                entity.HasIndex(e => e.ThesisId)
                    .HasName("IX_Urls_Thesis");

                entity.HasIndex(e => e.UrlType)
                    .HasName("IX_Urls_Type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArticleId).HasColumnName("article_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.TextbookId).HasColumnName("textbook_id");

                entity.Property(e => e.ThesisId).HasColumnName("thesis_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(2000);

                entity.Property(e => e.UrlType)
                    .HasColumnName("url_type")
                    .HasMaxLength(200);

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_ArticleURL_Article");

                entity.HasOne(d => d.Textbook)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.TextbookId)
                    .HasConstraintName("FK_Urls_Textbook");

                entity.HasOne(d => d.Thesis)
                    .WithMany(p => p.Urls)
                    .HasForeignKey(d => d.ThesisId)
                    .HasConstraintName("FK_Urls_Thesis");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.DeleteDate)
                    .HasName("IX_User_DeleteDate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CitizenshipId).HasColumnName("citizenship_id");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ImageAddress)
                    .HasColumnName("image_address")
                    .IsUnicode(false);

                entity.Property(e => e.IsOnline).HasColumnName("is_online");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(100);

                entity.Property(e => e.LastSeen)
                    .HasColumnName("last_seen")
                    .HasColumnType("datetime");

                entity.Property(e => e.NationalityId).HasColumnName("nationality_id");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(100);

                entity.Property(e => e.Patronymic)
                    .HasColumnName("patronymic")
                    .HasMaxLength(100);

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Citizenship)
                    .WithMany(p => p.UserCitizenship)
                    .HasForeignKey(d => d.CitizenshipId)
                    .HasConstraintName("FK_User_Country1");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.UserNationality)
                    .HasForeignKey(d => d.NationalityId)
                    .HasConstraintName("FK_User_Country");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
