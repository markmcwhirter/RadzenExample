using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LOBR.Models.LOBRCOnfiguration;

namespace LOBR.Data
{
    public partial class LOBRCOnfigurationContext : DbContext
    {
        public LOBRCOnfigurationContext()
        {
        }

        public LOBRCOnfigurationContext(DbContextOptions<LOBRCOnfigurationContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<LOBR.Models.LOBRCOnfiguration.Hrdatum> Hrdata { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}