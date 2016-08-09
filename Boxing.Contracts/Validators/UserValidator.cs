using Boxing.Contracts.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.fullName).Length(1, 32);
            RuleFor(x => x.username).NotEmpty().Length(1, 32);
            RuleFor(x => x.password).NotEmpty().Length(1, 32);
        }
    }
}
