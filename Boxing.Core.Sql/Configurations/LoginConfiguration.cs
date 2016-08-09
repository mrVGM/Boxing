using Boxing.Core.Sql.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Configurations
{
    public class LoginConfiguration : EntityTypeConfiguration<LoginEntity>
    {
        public LoginConfiguration()
        {
            ToTable("Login");
            Property(e => e.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.authToken).HasMaxLength(50).IsRequired();
            Property(e => e.expiration).IsRequired();
            
        }
    }
}   
