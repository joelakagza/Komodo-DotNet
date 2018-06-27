using Komodo.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Komodo.Data.Mapping
{
    public partial class ResultsRealMap : EntityTypeConfiguration<ResultsReal>
    {
        public ResultsRealMap()
        {
            this.ToTable("ResultsReal");
            this.HasKey(p => p.Id);
            this.Property(p => p.Env).IsRequired().HasMaxLength(50);
            this.Property(p => p.TestId).IsRequired().HasMaxLength(50);
            this.Property(p => p.Browser).IsRequired().HasMaxLength(50);
            this.Property(p => p.Scenario).IsRequired().HasMaxLength(500);
            
        }
    }
}