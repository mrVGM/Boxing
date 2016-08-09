using Boxing.Contracts.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Dto
{
    [Validator(typeof(MatchValidator))]
    public class Match
    {
        public int id { get; set; }
        public string boxer1 { get; set; }
        public string boxer2 { get; set; }
        public string place { get; set; }
        public DateTime dateOfMatch { get; set; }
        public string description { get; set; }
        public bool hasFinished { get; set; }
        public int winner { get; set; }
    }
}
