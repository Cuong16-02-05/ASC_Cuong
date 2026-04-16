using ASC.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASC.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<MasterDataKey> MasterDataKeys { get; set; }
        public DbSet<MasterDataValue> MasterDataValues { get; set; }

        // Lab 2 - Step 6: thêm Product model và cập nhật CSDL
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
