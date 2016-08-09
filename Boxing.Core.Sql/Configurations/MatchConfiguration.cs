using Boxing.Core.Sql.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Configurations
{
    public class MatchConfiguration : EntityTypeConfiguration<MatchEntity>
    {
        public MatchConfiguration()
        {
            ToTable("Match");
            Property(e => e.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.boxer1).HasMaxLength(32).IsRequired();
            Property(e => e.boxer2).HasMaxLength(32).IsRequired();
            Property(e => e.dateOfMatch).IsRequired();
            Property(e => e.description).HasMaxLength(2000).IsRequired();
            Property(e => e.place).HasMaxLength(50).IsRequired();
        }
    }
}
