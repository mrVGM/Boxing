using Boxing.Api.Services.Filters;
using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Core.Services.Exceptions;
using Boxing.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Boxing.Api.Services.Controllers
{
    public class LoginsController : ApiController
    {
        private readonly ILoginsService _loginsService;
        
        public LoginsController(ILoginsService loginsService)
        {
            _loginsService = loginsService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> create([FromBody] User request)
        {
            if (request.username == null || request.password == null)
            {
                throw new BadRequestException();
            }
            else
            {
                Object resp = await _loginsService.create(request).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.Created, resp);
            }
        }

        [UserAuth]
        [HttpDelete]
        public async Task<HttpResponseMessage> delete(int id)
        {
            await _loginsService.delete(id).ConfigureAwait(false);   
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
