using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Entities
{
    public class PredictionEntity
    {
        public int id { get; set; }
        public MatchEntity match { get; set; }
        public UserEntity user { get; set; }
        public int winner { get; set; }
    }
}
