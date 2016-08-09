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
    public class UserConfiguration : EntityTypeConfiguration<UserEntity>
    {
        public UserConfiguration()
        {
            ToTable("User");
            HasKey(e => e.username);
            Property(e => e.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.password).HasMaxLength(32).IsRequired();
            Property(e => e.username).HasMaxLength(32).IsRequired();
            Property(e => e.fullName).HasMaxLength(32).IsRequired();
        }
    }
}
