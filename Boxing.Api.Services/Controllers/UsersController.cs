using Boxing.Api.Services.Filters;
using Boxing.Contracts.Dto;
using Boxing.Core.Services.Exceptions;
using Boxing.Core.Services.Interfaces;
using Boxing.Core.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Boxing.Api.Services.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> createUser([FromBody] User user)
        {
            if (user.username == null || user.password == null)
            {
                throw new BadRequestException();
            }
            else
            {
                User resp = await _usersService.createUser(user).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.Created, resp);
            }
        }

        [UserAuth]
        [HttpGet]
        public async Task<IEnumerable<User>> getUsers([FromUri] int skip, [FromUri] int take, [FromUri] string sortBy, [FromUri] string order)
        {
            
            if (skip < 0 || take < 0 || (!sortBy.Equals("fullName") && !sortBy.Equals("rating")) || (!order.Equals("dsc") && !order.Equals("asc")))
            {
                throw new BadRequestException();
            }
            else
            {
                return await _usersService.getUsers(skip, take, sortBy, order).ConfigureAwait(false);
            }
        }

        [HttpGet]
        public async Task<User> getUser(int id)
        {
            return await _usersService.getUser(id).ConfigureAwait(false); 
        }

        [AdminAuth]
        [HttpDelete]
        public async Task<HttpResponseMessage> delete(int id)
        {
            await _usersService.delete(id).ConfigureAwait(false);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
