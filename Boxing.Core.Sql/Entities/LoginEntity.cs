using Boxing.Contracts.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Sql.Entities
{
    public class LoginEntity
    {
        public int id { get; set; }
        public UserEntity user { get; set; }
        public string authToken { get; set; }
        public DateTime expiration { get; set; }

        public static explicit operator Login(LoginEntity loginEntity)
        {
            Login l = new Login();
            l.id = loginEntity.id;
            l.authToken = JObject.FromObject(new { username = loginEntity.user.username, token = loginEntity.authToken }).ToString(Formatting.None);
            return l;
        }
    }
}
