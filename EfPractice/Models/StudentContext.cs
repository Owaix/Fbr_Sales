using EfPractice.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EfPractice.Models
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
    {


        public StudentContext(DbContextOptions<StudentContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Cate>(entity =>
            {
                entity.HasKey(e => e.Cid)
                    .HasName("PK__CATE__C1F8DC59886AFC77");

                entity.ToTable("CATE");

                entity.Property(e => e.Cid)
                    .ValueGeneratedNever()
                    .HasColumnName("CID");

                entity.Property(e => e.Aid)
                    .HasMaxLength(50)
                    .HasColumnName("AID");

                entity.Property(e => e.Aname)
                    .HasMaxLength(255)
                    .HasColumnName("ANAME");

                entity.Property(e => e.Mid).HasColumnName("MID");

                entity.Property(e => e.Mn)
                    .HasMaxLength(200)
                    .HasColumnName("MN");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("NAME");
            });



            builder.Entity<Imh>(entity =>
            {
                entity.HasKey(e => e.Mid)
                    .HasName("PK__IMH__C797348ADF1913BB");

                entity.ToTable("IMH");

                entity.Property(e => e.Mid).HasColumnName("MID");

                entity.Property(e => e.Mname)
                    .HasMaxLength(100)
                    .HasColumnName("MNAME");
            });

            builder.Entity<Party>(entity =>
            {
                entity.HasKey(e => e.Subcode)
                    .HasName("PK__PARTY__323701DC550C81F9");

                entity.ToTable("PARTY");

                entity.Property(e => e.Subcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUBCODE");

                entity.Property(e => e.Addr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FAX");

                entity.Property(e => e.Headcode).HasColumnName("HEADCODE");

                entity.Property(e => e.Limit).HasColumnName("LIMIT");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.Ntn)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("NTN");

                entity.Property(e => e.Ob)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("OB");

                entity.Property(e => e.Owner)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("OWNER");

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REGION");

                entity.Property(e => e.St)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("ST");

                entity.Property(e => e.Subname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SUBNAME");

                entity.Property(e => e.Telno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TELNO");

                entity.Property(e => e.Term).HasColumnName("TERM");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });


            builder.Entity<Supplier>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SUPPLIER");

                entity.Property(e => e.Addr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ADDR");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FAX");

                entity.Property(e => e.Headcode).HasColumnName("HEADCODE");

                entity.Property(e => e.Limit).HasColumnName("LIMIT");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.Ob)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("OB");

                entity.Property(e => e.Owner)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("OWNER");

                entity.Property(e => e.Region)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REGION");

                entity.Property(e => e.Subcode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SUBCODE");

                entity.Property(e => e.Subname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SUBNAME");

                entity.Property(e => e.Telno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TELNO");

                entity.Property(e => e.Term).HasColumnName("TERM");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            builder.Entity<City>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("city");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            builder.Entity<Head>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Head");

                entity.Property(e => e.HeaderId).HasColumnName("HeaderID");

                entity.Property(e => e.HeaderName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Opening).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Cate> Cates { get; set; } = null!;
        public DbSet<Imh> Imhs { get; set; } = null!;
        public virtual DbSet<Party> Parties { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Customer> customers { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Head> Heads { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;

    }
}
