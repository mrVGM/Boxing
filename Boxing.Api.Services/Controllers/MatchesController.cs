using Boxing.Api.Services.Filters;
using Boxing.Contracts;
using Boxing.Contracts.Dto;
using Boxing.Core.Services.Exceptions;
using Boxing.Core.Services.Interfaces;
using Boxing.Core.Sql;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Boxing.Core.Sql.Entities;

namespace Boxing.Api.Services.Controllers
{
    public class MatchesController : ApiController
    {
        private readonly IMatchesService _matchesService;
        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        [AdminAuth]
        [HttpPost]
        public async Task<HttpResponseMessage> create([FromBody] Match request)
        {
            if (request.boxer1 == null || request.boxer2 == null || request.dateOfMatch.CompareTo(DateTime.Now) < 0 || request.description == null || request.place == null)
            {
                throw new BadRequestException();
            }
            else
            {
                Object resp = await _matchesService.create(request).ConfigureAwait(false);
                return Request.CreateResponse(HttpStatusCode.Created, resp);
            }
        }

        [UserAuth]
        [HttpGet]
        public async Task<IEnumerable<Object>> getMatches([FromUri] int skip, [FromUri] int take)
        {
            if (skip < 0 || take < 0)
            {
                throw new BadRequestException();
            }
            else
            {
                return await _matchesService.getMatches(skip, take).ConfigureAwait(false);
            }
        }

        [UserAuth]
        [HttpGet]
        [Route("api/matches/{id}/predictions")]
        public async Task<Match> getPrediction(int id)
        {
            int loginId = Identifier.getUserToken(Request).login;
            using (var db = new BoxingContext())
            {
                LoginEntity login = db.Logins.Include("user").Where(x => x.id == loginId).FirstOrDefault();
                int prediction = _matchesService.getPrediction(id, login.user.username);
                return new Match { winner = prediction };
            }
        }

        [UserAuth]
        [HttpPost]
        [Route("api/matches/{id}/predictions")]
        public async Task<HttpResponseMessage> makePrediction([FromBody] Match mpr, [FromUri] int id)
        {
            int loginId = Identifier.getUserToken(Request).login;
            using (var db = new BoxingContext())
            {
                int matchId = id;
                var login = db.Logins.Include("user").Where(x => x.id == loginId).FirstOrDefault();
                string username = login.user.username;

                if (mpr.winner != 1 && mpr.winner != 2)
                {
                    throw new BadRequestException();
                }
                else
                {
                    await _matchesService.makePrediction(username, matchId, mpr.winner).ConfigureAwait(false);
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
        }

        [AdminAuth]
        [HttpPut]
        public async Task<Object> setWinner([FromBody] Match request)
        {
            if (request.winner != 1 && request.winner != 2)
            {
                throw new BadRequestException();
            }
            else
            {
                return await _matchesService.setWinner(request.id, request.winner).ConfigureAwait(false);
            }
        }

        [AdminAuth]
        [HttpDelete]
        public async Task<HttpResponseMessage> cancel(int id)
        {
            if (await _matchesService.cancel(id).ConfigureAwait(false))
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        [AdminAuth]
        [HttpGet]
        public async Task<IEnumerable<Object>> getOpen()
        {            
            return await _matchesService.getOpen();   
        }

        [HttpPut]
        [UserAuth]
        [Route("api/matches/{id}/predictions")]
        public async Task<HttpResponseMessage> changePrediction([FromBody] Match mpr, [FromUri] int id)
        {
            int loginId = Identifier.getUserToken(Request).login;

            BoxingContext db = new BoxingContext();

            string user = db.Logins.Find(loginId).user.username;
            

            if (mpr.winner != 1 && mpr.winner != 2)
            {
                throw new BadRequestException();
            }
            else
            {
                await _matchesService.changePrediction(user, id, mpr.winner);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }
        
        [UserAuth]
        [HttpGet]
        public async Task<IEnumerable<Object>> search(int skip, int take, string searchString)
        {
            if (skip < 0 || take < 0 || searchString == null)
            {
                throw new BadRequestException();
            }
            else {
                searchString = searchString.Split(new char[] { '"' })[1];
                if (searchString.Equals(""))
                {
                    return await _matchesService.getMatches(skip, take).ConfigureAwait(false);
                }
                return await _matchesService.search(skip, take, searchString).ConfigureAwait(false);
            }
        }

    }
}
