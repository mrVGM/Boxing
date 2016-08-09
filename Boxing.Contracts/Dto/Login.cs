using Boxing.Contracts.Validators;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Dto
{
    [Validator(typeof(LoginValidator))]
    public class Login
    {
        public int id { get; set; }
        public string authToken { get; set; }
        public string username { get; set; }
        public DateTime expiration { get; set; }
        public string encriptToken()
        {
            Object obj = new { login = id, token = authToken };
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }
    }
}
