using Boxing.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxing.Contracts.Dto;
using System.Data.Entity;
using Boxing.Core.Sql;
using Boxing.Core.Sql.Entities;
using Boxing.Core.Services.Exceptions;
using Boxing.Contracts;

namespace Boxing.Core.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly BoxingContext _db;

        public UsersService(BoxingContext db)
        {
            _db = db;
        } 

        public async Task<Contracts.Dto.User> createUser(Contracts.Dto.User request)
        {
            byte[] pass = new byte[request.password.Length * sizeof(char)];
            System.Buffer.BlockCopy(request.password.ToCharArray(), 0, pass, 0, request.password.Length);
            Sql.Entities.UserEntity user = new Sql.Entities.UserEntity
            {
                username = request.username,
                password = System.Text.Encoding.UTF8.GetString(System.Security.Cryptography.SHA256.Create().ComputeHash(pass)),
                fullName = request.fullName,
                correctPredictions = 0,
                wrongPredictions = 0,
                isAdmin = false,
                rating = 0
            };

            try {
                Sql.Entities.UserEntity entity = _db.Users.Add(user);
                _db.SaveChanges();
                
                return new Contracts.Dto.User{ id = entity.id, rating = entity.rating };
            }
            catch(Exception e)
            {
                _db.Users.Remove(user);
                throw new BadRequestException();
            }
        }

        public async Task<Unit> delete(int id)
        {
            Sql.Entities.UserEntity user = _db.Users.Where(x => x.id == id).FirstOrDefault();
            if (user == null)
            {
                throw new NotFoundException();
            }
            else
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Unit.Value;
            }
        }

        public async Task<Contracts.Dto.User> getUser(int id)
        {
            var user = _db.Users.Where(e => e.id == id).First();

            if (user == null)
            {
                throw new NotFoundException();
            }
            else
            {
                return new Contracts.Dto.User { id = user.id, username = user.username, fullName = user.fullName, rating = user.rating };
                //return new { userId = user.id, username = user.username, fullName = user.fullName, averageRating = user.rating };
                //return (UserInfo) user;
            }
        }

        public async Task<IEnumerable<Contracts.Dto.User>> getUsers(int skip, int take, string sortBy, string order)
        {

            Contracts.Dto.User [] users = { };
            if (sortBy.Equals("fullName"))
            {
                if (order.Equals("dsc"))
                {
                    users = _db.Users.OrderByDescending(e => e.fullName).Where(x => x.isAdmin == false).Skip(skip).Take(take).Select(x => new User{ id = x.id, username = x.username, fullName = x.fullName, rating = x.rating }).ToArray();
                }
                else
                {
                    try {
                        users = _db.Users.OrderBy(e => e.fullName).Where(x => x.isAdmin == false).Skip(skip).Take(take).Select(x => new User { id = x.id, username = x.username, fullName = x.fullName, rating = x.rating }).ToArray();
                    }
                    catch (Exception e)
                    {

                    }
                }
                
            }
            else
            {
                if (order.Equals("asc"))
                {
                    users = _db.Users.OrderByDescending(e => e.rating).Where(x => x.isAdmin == false).Skip(skip).Take(take).Select(x => new User { id = x.id, username = x.username, fullName = x.fullName, rating = x.rating }).ToArray();
                }
                else
                {
                    users = _db.Users.OrderBy(e => e.rating).Where(x => x.isAdmin == false).Skip(skip).Take(take).Select(x => new User { id = x.id, username = x.username, fullName = x.fullName, rating = x.rating }).ToArray();
                }
            }
            
            return users;
        }
    }
}
