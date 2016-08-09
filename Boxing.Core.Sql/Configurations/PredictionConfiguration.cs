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
    class PredictionConfiguration : EntityTypeConfiguration<PredictionEntity>
    {
        public PredictionConfiguration()
        {
            ToTable("Prediction");
            Property(e => e.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.winner).IsRequired();
        }
    }
}
