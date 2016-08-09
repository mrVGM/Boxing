using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
            predictions = new HashSet<PredictionEntity>();
        }
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
        public bool isAdmin { get; set; }
        public double rating { get; set; }
        public int correctPredictions { get; set; }
        public int wrongPredictions { get; set; }
        public virtual HashSet<PredictionEntity> predictions { get; set; }

        
        
    }
}
