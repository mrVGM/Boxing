﻿using Boxing.Core.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Boxing.Api.Services.Filters
{
    public class IncorrectCredentialsAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is IncorrectCredentialsException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new HttpError(actionExecutedContext.Exception.Message));
            }
        }
    }
}