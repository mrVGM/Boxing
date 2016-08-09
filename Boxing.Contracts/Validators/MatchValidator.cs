using Boxing.Contracts.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Validators
{
    class MatchValidator : AbstractValidator<Match>
    {
        public MatchValidator()
        {
            RuleFor(x => x.boxer1).Length(1, 32);
            RuleFor(x => x.boxer2).Length(1, 32);
            RuleFor(x => x.description).Length(1, 2000);
            RuleFor(x => x.place).Length(1, 50);
        }
    }
}
