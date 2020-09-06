using System;
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

        public virtual DbSet<AdministrativVezifeler> AdministrativVezifeler { get; set; }
        public virtual DbSet<ArasdirmaSaheleri> ArasdirmaSaheleri { get; set; }
        public virtual DbSet<ArasdirmaciAdministrativVezife> ArasdirmaciAdministrativVezife { get; set; }
        public virtual DbSet<ArasdirmaciDil> ArasdirmaciDil { get; set; }
        public virtual DbSet<ArasdirmaciMeqale> ArasdirmaciMeqale { get; set; }
        public virtual DbSet<ArasdirmaciPedoqojiAd> ArasdirmaciPedoqojiAd { get; set; }
        public virtual DbSet<Arasdirmacilar> Arasdirmacilar { get; set; }
        public virtual DbSet<ArasdirmacilarElmiJurnaldakiVezifeleri> ArasdirmacilarElmiJurnaldakiVezifeleri { get; set; }
        public virtual DbSet<BakalavriatSiyahi> BakalavriatSiyahi { get; set; }
        public virtual DbSet<DersArasdirmaci> DersArasdirmaci { get; set; }
        public virtual DbSet<Dersler> Dersler { get; set; }
        public virtual DbSet<DilSeviyye> DilSeviyye { get; set; }
        public virtual DbSet<Elanlar> Elanlar { get; set; }
        public virtual DbSet<Elaqe> Elaqe { get; set; }
        public virtual DbSet<ElmiJurnaldakiVezifeler> ElmiJurnaldakiVezifeler { get; set; }
        public virtual DbSet<ElmlerDoktorluguSiyahi> ElmlerDoktorluguSiyahi { get; set; }
        public virtual DbSet<ElmlerNamizedlikSiyahi> ElmlerNamizedlikSiyahi { get; set; }
        public virtual DbSet<Fakulteler> Fakulteler { get; set; }
        public virtual DbSet<IsTecrubesi> IsTecrubesi { get; set; }
        public virtual DbSet<Jurnallar> Jurnallar { get; set; }
        public virtual DbSet<Kafedralar> Kafedralar { get; set; }
        public virtual DbSet<MagistranturaSiyahisi> MagistranturaSiyahisi { get; set; }
        public virtual DbSet<Meqaleler> Meqaleler { get; set; }
        public virtual DbSet<MeslekiIdariDeneyim> MeslekiIdariDeneyim { get; set; }
        public virtual DbSet<Mukafatlar> Mukafatlar { get; set; }
        public virtual DbSet<Patentler> Patentler { get; set; }
        public virtual DbSet<Pdfler> Pdfler { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Sertifikatlar> Sertifikatlar { get; set; }
        public virtual DbSet<TehsilSeviyye> TehsilSeviyye { get; set; }
        public virtual DbSet<Universitetler> Universitetler { get; set; }
        public virtual DbSet<XariciDil> XariciDil { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-UTUBGGC\\SQLEXPRESS;Database=Aztu-Akademik;Trusted_Connection=True;");

                optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.LazyLoadOnDisposedContextWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdministrativVezifeler>(entity =>
            {
                entity.ToTable("Administrativ_vezifeler");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VezifeAd)
                    .IsRequired()
                    .HasColumnName("vezife_ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ArasdirmaSaheleri>(entity =>
            {
                entity.ToTable("Arasdirma_Saheleri");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_Id");

                entity.Property(e => e.KafedraId).HasColumnName("Kafedra_ID");

                entity.Property(e => e.SaheAd)
                    .HasColumnName("Sahe_ad")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.ArasdirmaSaheleri)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Arasdirma_Saheleri_Arasdirmacilar");

                entity.HasOne(d => d.Kafedra)
                    .WithMany(p => p.ArasdirmaSaheleri)
                    .HasForeignKey(d => d.KafedraId)
                    .HasConstraintName("FK_Arasdirma_Saheleri_kafedralar");
            });

            modelBuilder.Entity<ArasdirmaciAdministrativVezife>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Arasdirmaci_administrativ_vezife");

                entity.Property(e => e.AdministrativVezifeId).HasColumnName("Administrativ_vezife_ID");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.HasOne(d => d.AdministrativVezife)
                    .WithMany()
                    .HasForeignKey(d => d.AdministrativVezifeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Arasdirmaci_administrativ_vezife_Administrativ_vezifeler");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany()
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Arasdirmaci_administrativ_vezife_Arasdirmacilar");
            });

            modelBuilder.Entity<ArasdirmaciDil>(entity =>
            {
                entity.ToTable("Arasdirmaci_Dil");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_id");

                entity.Property(e => e.DilSeviyye).HasColumnName("dil_seviyye");

                entity.Property(e => e.XariciDilId).HasColumnName("xarici_dil_id");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.ArasdirmaciDil)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Arasdirmaci_Dil_Arasdirmacilar");

                entity.HasOne(d => d.DilSeviyyeNavigation)
                    .WithMany(p => p.ArasdirmaciDil)
                    .HasForeignKey(d => d.DilSeviyye)
                    .HasConstraintName("FK_Arasdirmaci_Dil_Dil_Seviyye");

                entity.HasOne(d => d.XariciDil)
                    .WithMany(p => p.ArasdirmaciDil)
                    .HasForeignKey(d => d.XariciDilId)
                    .HasConstraintName("FK_Arasdirmaci_Dil_Xarici_Dil");
            });

            modelBuilder.Entity<ArasdirmaciMeqale>(entity =>
            {
                entity.ToTable("Arasdirmaci_Meqale");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.Property(e => e.ElmiRehber).HasColumnName("Elmi_Rehber");

                entity.Property(e => e.MeqaleId).HasColumnName("Meqale_ID");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.ArasdirmaciMeqale)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Arasdirmaci_Meqale_Arasdirmacilar");

                entity.HasOne(d => d.Meqale)
                    .WithMany(p => p.ArasdirmaciMeqale)
                    .HasForeignKey(d => d.MeqaleId)
                    .HasConstraintName("FK_Arasdirmaci_Meqale_Meqaleler");
            });

            modelBuilder.Entity<ArasdirmaciPedoqojiAd>(entity =>
            {
                entity.ToTable("Arasdirmaci_pedoqoji_ad");

                entity.Property(e => e.ArasdirmaciPedoqojiAdId).HasColumnName("Arasdirmaci_pedoqoji_Ad_ID");

                entity.Property(e => e.ArasdirmaciAd)
                    .HasColumnName("Arasdirmaci_ad")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Arasdirmacilar>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArasdirmaciAd)
                    .HasColumnName("Arasdirmaci_Ad")
                    .HasMaxLength(50);

                entity.Property(e => e.ArasdirmaciEmeil)
                    .HasColumnName("Arasdirmaci_emeil")
                    .HasMaxLength(50);

                entity.Property(e => e.ArasdirmaciPassword)
                    .HasColumnName("Arasdirmaci_password")
                    .HasMaxLength(50);

                entity.Property(e => e.ArasdirmaciPedoqojiAdId).HasColumnName("Arasdirmaci_pedoqoji_ad_ID");

                entity.Property(e => e.ArasdirmaciSoyad)
                    .HasColumnName("Arasdirmaci_Soyad")
                    .HasMaxLength(50);

                entity.Property(e => e.CvAdres)
                    .HasColumnName("cv_adres")
                    .HasMaxLength(500);

                entity.Property(e => e.KafedraId).HasColumnName("Kafedra_ID");

                entity.Property(e => e.MeslekiIdariDeneyimID).HasColumnName("Mesleki_Idari_Deneyim_iD");

                entity.Property(e => e.ProfilShekil)
                    .HasColumnName("profil_shekil")
                    .HasMaxLength(500);

                entity.Property(e => e.RolId).HasColumnName("rol_id");

                entity.Property(e => e.TehsilSeviyyesiId).HasColumnName("Tehsil_seviyyesi_ID");

                entity.HasOne(d => d.ArasdirmaciPedoqojiAd)
                    .WithMany(p => p.Arasdirmacilar)
                    .HasForeignKey(d => d.ArasdirmaciPedoqojiAdId)
                    .HasConstraintName("FK_Arasdirmacilar_Arasdirmaci_Pedoqoji_Ad");

                entity.HasOne(d => d.Kafedra)
                    .WithMany(p => p.Arasdirmacilar)
                    .HasForeignKey(d => d.KafedraId)
                    .HasConstraintName("FK_Arasdirmacilar_kafedralar");

                entity.HasOne(d => d.MeslekiIdariDeneyim)
                    .WithMany(p => p.Arasdirmacilar)
                    .HasForeignKey(d => d.MeslekiIdariDeneyimID)
                    .HasConstraintName("FK_Arasdirmacilar_Mesleki_Idari_Deneyim");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Arasdirmacilar)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_Arasdirmacilar_Rol");
            });

            modelBuilder.Entity<ArasdirmacilarElmiJurnaldakiVezifeleri>(entity =>
            {
                entity.ToTable("Arasdirmacilar_Elmi_jurnaldaki_vezifeleri");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_ID");

                entity.Property(e => e.ElmiJurnaldakiVezifeId).HasColumnName("elmi_jurnaldaki_vezife_ID");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.ArasdirmacilarElmiJurnaldakiVezifeleri)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Arasdirmacilar_Elmi_jurnaldaki_vezifeleri_Arasdirmacilar");

                entity.HasOne(d => d.ElmiJurnaldakiVezife)
                    .WithMany(p => p.ArasdirmacilarElmiJurnaldakiVezifeleri)
                    .HasForeignKey(d => d.ElmiJurnaldakiVezifeId)
                    .HasConstraintName("FK_Arasdirmacilar_Elmi_jurnaldaki_vezifeleri_Elmi_jurnaldaki_vezifeler");
            });

            modelBuilder.Entity<BakalavriatSiyahi>(entity =>
            {
                entity.ToTable("Bakalavriat_siyahi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BakalavrBaslangicIl)
                    .HasColumnName("Bakalavr_baslangic_il")
                    .HasColumnType("date");

                entity.Property(e => e.BakalavrBitisIl)
                    .HasColumnName("Bakalavr_bitis_il")
                    .HasColumnType("date");

                entity.Property(e => e.BakalavrDisertasiyaAd)
                    .HasColumnName("Bakalavr_disertasiya_ad")
                    .HasMaxLength(70);

                entity.Property(e => e.BakalavrDisertasiyaPdfId).HasColumnName("Bakalavr_disertasiya_PDF_ID");

                entity.Property(e => e.BakalavrUniversitet).HasColumnName("Bakalavr_Universitet");
            });

            modelBuilder.Entity<DersArasdirmaci>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Ders_arasdirmaci");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.Property(e => e.DersId).HasColumnName("ders_ID");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany()
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ders_arasdirmaci_Arasdirmacilar");

                entity.HasOne(d => d.Ders)
                    .WithMany()
                    .HasForeignKey(d => d.DersId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ders_arasdirmaci_Dersler");
            });

            modelBuilder.Entity<Dersler>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DersSeviyye)
                    .HasColumnName("Ders_Seviyye")
                    .HasMaxLength(20);

                entity.Property(e => e.KafedraId).HasColumnName("Kafedra_ID");

                entity.HasOne(d => d.Kafedra)
                    .WithMany(p => p.Dersler)
                    .HasForeignKey(d => d.KafedraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dersler_kafedralar");
            });

            modelBuilder.Entity<DilSeviyye>(entity =>
            {
                entity.ToTable("Dil_Seviyye");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SeviyyeAd)
                    .HasColumnName("seviyye_ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Elanlar>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Aciqlama).HasColumnName("aciqlama");

                entity.Property(e => e.Ad)
                    .HasColumnName("ad")
                    .HasMaxLength(200);

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_id");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.Elanlar)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Elanlar_Arasdirmacilar");
            });

            modelBuilder.Entity<Elaqe>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .HasMaxLength(500);

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .HasMaxLength(500);

                entity.Property(e => e.Linkedin)
                    .HasColumnName("linkedin")
                    .HasMaxLength(500);

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasMaxLength(100);

                entity.Property(e => e.WebSite)
                    .HasColumnName("web_site")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.Elaqe)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Elaqe_Arasdirmacilar");
            });

            modelBuilder.Entity<ElmiJurnaldakiVezifeler>(entity =>
            {
                entity.ToTable("Elmi_jurnaldaki_vezifeler");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VezifeAd)
                    .HasColumnName("Vezife_ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ElmlerDoktorluguSiyahi>(entity =>
            {
                entity.ToTable("Elmler_doktorlugu_siyahi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ElmlerDoktoruBaslangicIl)
                    .HasColumnName("elmler_doktoru_baslangic_il")
                    .HasColumnType("date");

                entity.Property(e => e.ElmlerDoktoruBitisIl)
                    .HasColumnName("elmler_doktoru_bitis_il")
                    .HasColumnType("date");

                entity.Property(e => e.ElmlerDoktoruDisertasiyaAd)
                    .HasColumnName("elmler_doktoru_disertasiya_ad")
                    .HasMaxLength(500);

                entity.Property(e => e.ElmlerDoktoruDisertasiyaPdfId).HasColumnName("elmler_doktoru__disertasiya_PDF_ID");

                entity.Property(e => e.ElmlerDoktoruUniversitetId).HasColumnName("elmler_doktoru_universitet_ID");
            });

            modelBuilder.Entity<ElmlerNamizedlikSiyahi>(entity =>
            {
                entity.ToTable("elmler_namizedlik_siyahi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ElmlerNamizediBaslangicIl)
                    .HasColumnName("elmler_namizedi_baslangic_il")
                    .HasColumnType("date");

                entity.Property(e => e.ElmlerNamizediBitisIl)
                    .HasColumnName("elmler_namizedi_bitis_il")
                    .HasColumnType("date");

                entity.Property(e => e.ElmlerNamizediDisertasiyaAd)
                    .IsRequired()
                    .HasColumnName("elmler_namizedi_disertasiya_ad")
                    .HasMaxLength(100);

                entity.Property(e => e.ElmlerNamizediDisertasiyaPdfId).HasColumnName("elmler_namizedi_disertasiya_PDF_ID");

                entity.Property(e => e.ElmlerNamizediUniversitetId).HasColumnName("elmler_namizedi_universitet_ID");
            });

            modelBuilder.Entity<Fakulteler>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FakulteAd)
                    .HasColumnName("Fakulte_Ad")
                    .HasMaxLength(10);

                entity.Property(e => e.KafedraId).HasColumnName("Kafedra_ID");

                entity.HasOne(d => d.Kafedra)
                    .WithMany(p => p.Fakulteler)
                    .HasForeignKey(d => d.KafedraId)
                    .HasConstraintName("FK_Fakulteler_kafedralar");
            });

            modelBuilder.Entity<IsTecrubesi>(entity =>
            {
                entity.ToTable("Is_tecrubesi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_id");

                entity.Property(e => e.IsBaslangicTarixi)
                    .HasColumnName("Is_baslangic_tarixi")
                    .HasColumnType("date");

                entity.Property(e => e.IsBitisTarixi)
                    .HasColumnName("Is_bitis_tarixi")
                    .HasColumnType("date");

                entity.Property(e => e.IsVezife)
                    .IsRequired()
                    .HasColumnName("Is_vezife")
                    .HasMaxLength(50);

                entity.Property(e => e.IsYeri)
                    .HasColumnName("Is_yeri")
                    .HasColumnType("text");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.IsTecrubesi)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Is_tecrubesi_Arasdirmacilar");
            });

            modelBuilder.Entity<Jurnallar>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JurnalAd)
                    .HasColumnName("Jurnal_ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Kafedralar>(entity =>
            {
                entity.ToTable("kafedralar");

                entity.Property(e => e.KafedraAd)
                    .HasColumnName("Kafedra_Ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MagistranturaSiyahisi>(entity =>
            {
                entity.ToTable("Magistrantura_siyahisi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MagistrBaslangicIl)
                    .HasColumnName("Magistr_baslangic_il")
                    .HasColumnType("date");

                entity.Property(e => e.MagistrBitisIl)
                    .HasColumnName("Magistr_bitis_il")
                    .HasColumnType("date");

                entity.Property(e => e.MagistrDisertasiyaAd)
                    .IsRequired()
                    .HasColumnName("Magistr_disertasiya_ad")
                    .HasMaxLength(70);

                entity.Property(e => e.MagistrDisertasiyaPdfId).HasColumnName("Magistr_disertasiya_PDF_ID");

                entity.Property(e => e.MagistrUniversitetId).HasColumnName("Magistr_Universitet_ID");
            });

            modelBuilder.Entity<Meqaleler>(entity =>
            {
                entity.Property(e => e.MeqaleAd)
                    .HasColumnName("Meqale_Ad")
                    .HasMaxLength(50);

                entity.Property(e => e.MeqaleIl)
                    .HasColumnName("Meqale_il")
                    .HasColumnType("date");

                entity.Property(e => e.MeqaleJurnalId).HasColumnName("Meqale_jurnal_ID");

                entity.Property(e => e.MeqaleNovId)
                    .HasColumnName("Meqale_nov_Id")
                    .HasMaxLength(20);

                entity.Property(e => e.Olke).HasMaxLength(60);

                entity.Property(e => e.SaheId).HasColumnName("Sahe_Id");

                entity.Property(e => e.UniversitetId).HasColumnName("Universitet_Id");

                entity.HasOne(d => d.MeqaleJurnal)
                    .WithMany(p => p.Meqaleler)
                    .HasForeignKey(d => d.MeqaleJurnalId)
                    .HasConstraintName("FK_Meqale_Jurnallar");

                entity.HasOne(d => d.Universitet)
                    .WithMany(p => p.Meqaleler)
                    .HasForeignKey(d => d.UniversitetId)
                    .HasConstraintName("FK_Meqale_Universitetler");
            });

            modelBuilder.Entity<MeslekiIdariDeneyim>(entity =>
            {
                entity.ToTable("Mesleki_Idari_Deneyim");

                entity.Property(e => e.MeslekiIdariDeneyimAd)
                    .HasColumnName("Mesleki-idari_Deneyim_Ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Mukafatlar>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.Property(e => e.MukafatAd)
                    .IsRequired()
                    .HasColumnName("Mukafat_ad")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.Mukafatlar)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mukafatlar_Arasdirmacilar");
            });

            modelBuilder.Entity<Patentler>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.Property(e => e.PatentAd)
                    .HasColumnName("Patent_Ad")
                    .HasMaxLength(60);

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.Patentler)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patentler_Arasdirmacilar");
            });

            modelBuilder.Entity<Pdfler>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MeqaleId).HasColumnName("Meqale_ID");

                entity.Property(e => e.PdfLocation)
                    .IsRequired()
                    .HasColumnName("PDF_Location")
                    .HasColumnType("text");

                entity.HasOne(d => d.Meqale)
                    .WithMany(p => p.Pdfler)
                    .HasForeignKey(d => d.MeqaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pdfler_Meqale");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RolAd)
                    .HasColumnName("rol_ad")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Sertifikatlar>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Aciqlama).HasMaxLength(200);

                entity.Property(e => e.ArasdirmaciId).HasColumnName("Arasdirmaci_ID");

                entity.Property(e => e.IsIndexed).HasColumnName("isIndexed");

                entity.Property(e => e.PdfAdres).HasColumnName("pdf_adres");

                entity.Property(e => e.SertifikatAd)
                    .IsRequired()
                    .HasColumnName("Sertifikat_ad")
                    .HasMaxLength(70);

                entity.Property(e => e.SertifikatLink)
                    .HasColumnName("sertifikat_link")
                    .HasMaxLength(500);

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.Sertifikatlar)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sertifikatlar_Arasdirmacilar");
            });

            modelBuilder.Entity<TehsilSeviyye>(entity =>
            {
                entity.ToTable("Tehsil_seviyye");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArasdirmaciId).HasColumnName("arasdirmaci_id");

                entity.Property(e => e.BakalavrId).HasColumnName("Bakalavr_ID");

                entity.Property(e => e.ElmlerDoktoru).HasColumnName("elmler_doktoru");

                entity.Property(e => e.ElmlerNamizediId).HasColumnName("elmler_namizedi_ID");

                entity.Property(e => e.MagistrId).HasColumnName("Magistr_ID");

                entity.HasOne(d => d.Arasdirmaci)
                    .WithMany(p => p.TehsilSeviyye)
                    .HasForeignKey(d => d.ArasdirmaciId)
                    .HasConstraintName("FK_Tehsil_seviyye_Arasdirmacilar");

                entity.HasOne(d => d.Bakalavr)
                    .WithMany(p => p.TehsilSeviyye)
                    .HasForeignKey(d => d.BakalavrId)
                    .HasConstraintName("FK_Tehsil_seviyye_Bakalavriat_siyahi");

                entity.HasOne(d => d.ElmlerDoktoruNavigation)
                    .WithMany(p => p.TehsilSeviyye)
                    .HasForeignKey(d => d.ElmlerDoktoru)
                    .HasConstraintName("FK_Tehsil_seviyye_Elmler_doktorlugu_siyahi");

                entity.HasOne(d => d.ElmlerNamizedi)
                    .WithMany(p => p.TehsilSeviyye)
                    .HasForeignKey(d => d.ElmlerNamizediId)
                    .HasConstraintName("FK_Tehsil_seviyye_elmler_namizedlik_siyahi1");

                entity.HasOne(d => d.Magistr)
                    .WithMany(p => p.TehsilSeviyye)
                    .HasForeignKey(d => d.MagistrId)
                    .HasConstraintName("FK_Tehsil_seviyye_Magistrantura_siyahisi");
            });

            modelBuilder.Entity<Universitetler>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UniversitetAd)
                    .HasColumnName("Universitet_Ad")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<XariciDil>(entity =>
            {
                entity.ToTable("Xarici_Dil");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad)
                    .HasColumnName("ad")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
