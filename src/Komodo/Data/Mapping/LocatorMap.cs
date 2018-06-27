using Komodo.Core.Domain;
using System.Data.Entity.ModelConfiguration;


namespace Komodo.Data.Mapping
{
    public partial class LocatorMap : EntityTypeConfiguration<Locator>
    {
        public LocatorMap()
        {
            this.ToTable("locator");
            this.HasKey(p => p.Id);
            this.Property(p => p.tcId).IsRequired().HasMaxLength(500);
            this.Property(p => p.type).IsRequired().HasMaxLength(50);
            this.Property(p => p.byName).IsRequired().HasMaxLength(400);
            this.Property(p => p.byValue).IsRequired().HasMaxLength(400);
            this.Property(p => p.keyName).IsRequired().HasMaxLength(400);
            this.Property(p => p.page).IsRequired().HasMaxLength(500);
            this.Property(p => p.description).IsRequired().HasMaxLength(4000);
        }
    }
}