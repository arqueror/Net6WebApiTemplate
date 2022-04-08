using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using webapi.Data.Interfaces;

namespace webapi.Data
{
    public class PostgresContext : DbContext, IPostgresUnitOfWork
    {
        //public virtual DbSet<ApexJobs> ApexJobs { get; set; }
        //public virtual DbSet<CorrectedImages> CorrectedImages { get; set; }
        //public virtual DbSet<CorrectedIndexReadings> CorrectedIndexReadings { get; set; }
        //public virtual DbSet<Ftplog> Ftplog { get; set; }
        //Use a POCO generator for create all the POCO and config files

        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        {

        }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();

        }

        public EntityEntry Entry<T>(T o) where T : class
        {
            return base.Entry<T>(o);
        }

        public bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure entities mappings
            //modelBuilder.Entity<ApexJobs>(entity =>
            //{
            //    entity.HasKey(e => e.ApexJobId);

            //    entity.Property(e => e.ApexJobId).HasColumnName("ApexJobID");

            //    entity.Property(e => e.ApexClientName).HasMaxLength(200);

            //    entity.Property(e => e.ApexJobNumber).HasMaxLength(50);

            //    entity.Property(e => e.DatabaseName).HasMaxLength(50);
            //});
        }
    }
}
