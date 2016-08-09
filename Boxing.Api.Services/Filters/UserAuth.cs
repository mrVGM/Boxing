using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using Boxing.Core.Sql;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Boxing.Core.Sql.Entities;
using System;

namespace Boxing.Api.Services.Filters
{
    public class UserAuth : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var tok = Identifier.getUserToken(actionContext.Request);
            if (tok == null || tok.token == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                base.OnAuthorization(actionContext);
                return;
            }

            if (Identifier.getLoginId(actionContext.Request) != -1 && Identifier.getLoginId(actionContext.Request) != tok.login)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                base.OnAuthorization(actionContext);
                return;
            }

            using (var _db = new BoxingContext()) {
                LoginEntity entity = _db.Logins.Find(tok.login);
                if (entity == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    base.OnAuthorization(actionContext);
                    return;
                }

                if (tok.token.Equals(entity.authToken) && DateTime.Now.CompareTo(entity.expiration) < 0)
                {
                    entity.expiration = DateTime.Now.AddMinutes(10);
                    _db.SaveChanges();
                    base.OnAuthorization(actionContext);
                    return;
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            base.OnAuthorization(actionContext);
        }
    }
}