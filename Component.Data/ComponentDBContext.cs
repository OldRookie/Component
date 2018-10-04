using Component.Model.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;

namespace Component.Data
{
    public class ComponentDBContext: DbContext
    {

        public ComponentDBContext() :base(("ComponentDBContext")) {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            //Database.SetInitializer<ComponentDBContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LocaleResource> LocaleResource { get; set; }
        public DbSet<BaseForm> BaseForm { get; set; }
        public DbSet<FormDetail> FormDetail { get; set; }
    }
}
