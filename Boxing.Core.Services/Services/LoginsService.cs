using Boxing.Core.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxing.Contracts.Dto;
using Boxing.Contracts;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Entities;
using Boxing.Core.Services.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boxing.Core.Services.Services
{
    public class LoginsService : ILoginsService
    {
        private readonly BoxingContext _db;

        public LoginsService(BoxingContext db)
        {
            _db = db;
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private string generateToken(LoginEntity l)
        {
            return JObject.FromObject(new { id = l.id, username = l.user.username, token = l.authToken })
                   .ToString(Formatting.None);
        }

        private bool authenticate(string username, string password)
        {
            UserEntity user = _db.Users.Find(username);
            if (user == null)
            {
                return false;
            }

            byte[] pass = new byte[password.Length * sizeof(char)];
            System.Buffer.BlockCopy(password.ToCharArray(), 0, pass, 0, password.Length);

            if (user.password.Equals(System.Text.Encoding.UTF8.GetString(System.Security.Cryptography.SHA256.Create().ComputeHash(pass))))
            {
                return true;
            }
            return false;
        }

        public async Task<Object> create(User request)
        {
            Sql.Entities.UserEntity user = _db.Users.Find(request.username);

            if (user == null || !authenticate(request.username, request.password))
            {
                throw new IncorrectCredentialsException();
            }
            else
            {
                LoginEntity login = _db.Logins.Where(e => e.user.username == request.username).FirstOrDefault();
                if (login == null)
                {
                    login = new LoginEntity()
                    {
                        user = user,
                        authToken = RandomString(50),
                        expiration = DateTime.Now.AddMinutes(10),
                    };

                    login = _db.Logins.Add(login);
                }
                else
                {
                    login.expiration = DateTime.Now.AddMinutes(10);
                }

                _db.SaveChanges();

                if (login.user.isAdmin)
                {
                    return new { id = login.id, adminToken = login.authToken };
                }
                else
                {
                    return new { id = login.id, authToken = login.authToken };
                }
                
            }
            
        }
        
        public async Task<Unit> delete(int id)
        {
            LoginEntity login = _db.Logins.Where(e => e.id == id).FirstOrDefault();
            if (login != null)
            {
                _db.Logins.Remove(login);
                _db.SaveChanges();
            }
            return Unit.Value;
        }
    }
}
