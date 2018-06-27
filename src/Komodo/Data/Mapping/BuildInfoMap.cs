using Komodo.Core.Domain;
using System.Data.Entity.ModelConfiguration;


namespace Komodo.Data.Mapping
{
    public partial class BuildInfoMap : EntityTypeConfiguration<BuildInfo>
    {
        public BuildInfoMap()
        {
            this.ToTable("BuildInfo");
            this.HasKey(p => p.Id);
            this.Property(p => p.Env).IsRequired().HasMaxLength(50);
            this.Property(p => p.ProjectName).IsRequired().HasMaxLength(250);
            this.Property(p => p.BuildNo).IsRequired().HasMaxLength(50);
            this.Property(p => p.BuildName).IsRequired().HasMaxLength(400);
        }
    }
}
