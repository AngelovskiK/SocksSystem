using BossSystem.Models;
using BossSystem.Models.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BossSystem.Database
{
    public class ApplicationDbContext: DbContext
    {
        IConfiguration Configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteConfiguration>().HasData(new SiteConfiguration { 
                Key = Configuration.GetSection("ConfigurationKeys")["AdminPasswordConfigurationKey"], Value = "TheBoss" 
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SiteConfiguration> SiteConfigurations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

