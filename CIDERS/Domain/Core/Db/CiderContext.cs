using CIDERS.Domain.Core.Entity.Cider;
using Microsoft.EntityFrameworkCore;


namespace CIDERS.Domain.Core.Db;
public class CiderContext : DbContext
    {
    public CiderContext(DbContextOptions<CiderContext> options) : base(options)
    {
    }

    public DbSet<AuditLog>? AuditLog { get; set; }
        public DbSet<ApiUser>? ApiUser { get; set; }
        public DbSet<Branch>? Branch { get; set; }
        public DbSet<ApiPermission>? ApiPermission { get; set; }
        public DbSet<ApiUserPermission>? ApiUserPermission { get; set; }
        public DbSet<ApiRank>? ApiRank { get; set; }
        public DbSet<ApiDepartment>? ApiDepartment { get; set; }
        public DbSet<ApiLocation>? ApiLocation { get; set; }
        public DbSet<ApiEmployee>? ApiEmployee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApiUserPermission>().Navigation(e => e.FkPermission).AutoInclude();
            modelBuilder.Entity<ApiUserPermission>().Navigation(e => e.FkUser).AutoInclude();
            modelBuilder.Entity<ApiEmployee>().Navigation(e => e.FkRank).AutoInclude();
            modelBuilder.Entity<ApiEmployee>().Navigation(e => e.FkDept).AutoInclude();
            modelBuilder.Entity<ApiEmployee>().Navigation(e => e.FkLocation).AutoInclude();
    }
   }

