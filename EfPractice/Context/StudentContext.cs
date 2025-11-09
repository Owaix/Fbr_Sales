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
        public virtual DbSet<Tax> Taxes { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!; // added
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentContext(DbContextOptions<StudentContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

public interface IHasCompany
{
    int CompanyId { get; set; }
}