using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Boxing
{
    public class TokenHeader
    {
        public static HttpRequestMessage addTokenHeader(HttpCookieCollection cookies, HttpRequestMessage apiRequest)
        {
            if (cookies.Get("adminToken") != null)
            {
                string token = JsonConvert.SerializeObject(new { login = int.Parse(cookies.Get("id").Value), token = cookies.Get("adminToken").Value });
                apiRequest.Headers.Add("adminToken", token);
                apiRequest.Headers.Add("authToken", token);
                return apiRequest;
            }
            if (cookies.Get("authToken") != null)
            {
                string token = JsonConvert.SerializeObject(new { login = int.Parse(cookies.Get("id").Value), token = cookies.Get("authToken").Value });
                apiRequest.Headers.Add("authToken", token);
                return apiRequest;
            }
            return apiRequest;
        } 
    }
}