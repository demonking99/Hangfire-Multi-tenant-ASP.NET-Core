using Core.Contracts;
using Core.Entities;
using Core.Hangfire.Interfaces;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;
        private readonly IHangfireTenantProvider _hfTenantProvider;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService, IHangfireTenantProvider hfTenantProvider) : base(options)
        {
            if (tenantService.GetTenant() == null && !string.IsNullOrWhiteSpace(hfTenantProvider.HfGetTenantId()))
            {
                tenantService.SetTenant(hfTenantProvider.HfGetTenantId());
            }
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TID;
            _hfTenantProvider = hfTenantProvider;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(a => a.TenantId == TenantId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "sqlserver")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = base.SaveChanges();
            return result;
        }
    }
}