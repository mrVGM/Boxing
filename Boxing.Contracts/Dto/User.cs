using Boxing.Contracts.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Dto
{
    [Validator(typeof(UserValidator))]
    public class User
    {
        [DisplayName("Username")]
        public string username { get; set; }
        [DisplayName("Password")]
        public string password { get; set; }
        [DisplayName("Full Name")]
        public string fullName { get; set; }
        public int id { get; set; }
        public double rating { get; set; } 
    }
}
