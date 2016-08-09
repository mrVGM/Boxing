namespace Boxing.Core.Sql.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Boxing.Core.Sql.BoxingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Boxing.Core.Sql.BoxingContext context)
        {
            /*byte[] pass = new byte["admin".Length * sizeof(char)];
            System.Buffer.BlockCopy("admin".ToCharArray(), 0, pass, 0, "admin".Length);

            UserEntity user = new UserEntity
            {
                username = "admin",
                fullName = "admin",
                password = System.Text.Encoding.UTF8.GetString(System.Security.Cryptography.SHA256.Create().ComputeHash(pass)),
                isAdmin = true
            };
            context.Users.Add(user);
            context.SaveChanges();*/
        }
    }
}
