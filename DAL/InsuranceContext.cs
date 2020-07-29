using Insurance.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Insurance.DAL
{
    /// <summary>
    /// Insurance context class.
    /// </summary>
    public class InsuranceContext : DbContext
    {
        /// <summary>
        /// Insurance context constructor.
        /// </summary>
        public InsuranceContext() : base("InsuranceContext")
        {
        }

        /// <summary>
        /// Clients collection.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Coverages collection.
        /// </summary>
        public DbSet<Coverage> Coverages { get; set; }

        /// <summary>
        /// Policies collection.
        /// </summary>
        public DbSet<Policy> Policies { get; set; }

        /// <summary>
        /// On model creating event.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}