using Boxing.Core.Sql;
using Boxing.Core.Sql.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Boxing.Api.Services.Filters
{
    public class AdminAuth : AuthorizationFilterAttribute
    {   
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var tok = Identifier.getAdminToken(actionContext.Request);
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
            using (var _db = new BoxingContext())
            {
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