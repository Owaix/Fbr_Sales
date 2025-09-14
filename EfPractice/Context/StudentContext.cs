using EfPractice.Areas.Identity.Data;
using EfPractice.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EfPractice.Context
{
    public class StudentContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Cate> Cates { get; set; } = null!;
        public DbSet<Imh> Imhs { get; set; } = null!;
        public virtual DbSet<Party> Parties { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Head> Heads { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<SaleInvoice> SaleInvoices { get; set; } = null!;
        public virtual DbSet<SaleInvoiceItem> SaleInvoiceItems { get; set; } = null!;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentContext(DbContextOptions<StudentContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var companyId = GetCompanyId();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IHasCompany).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(IHasCompany.CompanyId));
                    var companyIdConstant = Expression.Constant(companyId);
                    var equal = Expression.Equal(property, companyIdConstant);

                    // Build lambda: e => e.CompanyId == companyId
                    var lambda = Expression.Lambda(
                        typeof(Func<,>).MakeGenericType(entityType.ClrType, typeof(bool)),
                        equal,
                        parameter
                    );

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override int SaveChanges()
        {
            SetCompanyId();
            return base.SaveChanges();
        }
        private int GetCompanyId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCompanyId();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetCompanyId()
        {
            var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
            if (int.TryParse(companyIdClaim, out var companyId))
            {
                foreach (var entry in ChangeTracker.Entries()
                             .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.Entity is IHasCompany entityWithCompany)
                    {
                        entityWithCompany.CompanyId = companyId;
                    }
                }
            }
        }
    }
}

public interface IHasCompany
{
    int CompanyId { get; set; }
}