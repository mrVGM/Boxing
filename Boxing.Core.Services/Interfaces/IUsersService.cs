using Boxing.Contracts;
using Boxing.Contracts.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> createUser(User request);
        Task<IEnumerable<User>> getUsers(int skip, int take, string sortBy, string order);
        Task<User> getUser(int id);
        Task<Unit> delete(int id);
    }
}
