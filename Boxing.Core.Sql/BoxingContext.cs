using Boxing.Core.Sql.Configurations;
using Boxing.Core.Sql.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Boxing.Core.Sql
{
    public class BoxingContext : DbContext
    {
        public BoxingContext()
            : base("BoxingContext") { }
        public DbSet<LoginEntity> Logins { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<PredictionEntity> Predictions { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new LoginConfiguration());
            modelBuilder.Configurations.Add(new MatchConfiguration());
            modelBuilder.Configurations.Add(new PredictionConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }

        public static void SetInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BoxingContext, Migrations.Configuration>());
        }
        
        public void deletePredictions(int matchId)
        {
            IEnumerable<PredictionEntity> predictions = Predictions.Include("match").Where(x => x.match.id == matchId);

            foreach (PredictionEntity p in predictions)
            {
                Predictions.Remove(p);
            }
            var match = Matches.Find(matchId);
            if (match != null)
            {
                Matches.Remove(match);
            }
            SaveChanges();
        }
        public void deletePredictionsAsync(int matchId)
        {
            new Thread(() => deletePredictions(matchId)).Start();
        }
        
        public void updateRatings(int matchId, int winner)
        {
            IEnumerable<PredictionEntity> predictions = Predictions.Include("match").Include("user").Where(x => x.match.id == matchId);

            foreach (PredictionEntity p in predictions)
            {
                if (p.winner == winner)
                {
                    p.user.correctPredictions++;
                }
                else
                {
                    p.user.wrongPredictions++;
                }
                if (p.user.correctPredictions + p.user.wrongPredictions != 0)
                {
                    p.user.rating = ((double)p.user.correctPredictions) / (p.user.correctPredictions + p.user.wrongPredictions);
                }
                else
                {
                    p.user.rating = 0;
                }
            }
            SaveChanges();
        }

        public void updateRatinsAsync(int matchId, int winner)
        {
            new Thread(() => updateRatings(matchId, winner)).Start();
        }
    }
}
