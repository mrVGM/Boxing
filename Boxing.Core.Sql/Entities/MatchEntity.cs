using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Entities
{
    public class MatchEntity
    {
        public int id { get; set; }
        public string boxer1 { get; set; }
        public string boxer2 { get; set; }
        public string place { get; set; }
        public DateTime dateOfMatch { get; set; }
        public string description { get; set; }
        public int winner { get; set; }
        public bool isClosed { get; set; }

    }
}
