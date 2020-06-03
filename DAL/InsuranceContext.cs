using Insurance.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Insurance.DAL
{
    public class InsuranceContext : DbContext
    {

        public InsuranceContext() : base("InsuranceContext")
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Coverage> Coverages { get; set; }
        public DbSet<Policy> Policies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}