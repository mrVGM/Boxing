using Boxing.Contracts.Dto;
using Boxing.Core.Sql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Boxing.Api.Services
{
    public class Identifier
    {
        public class Tok
        {
            public int login { get; set; }
            public string token { get; set; }
        }
        public static Tok getUserToken(HttpRequestMessage request)
        {
            if (request.Headers.Contains("authToken"))
            {
                return JsonConvert.DeserializeObject<Tok>(request.Headers.GetValues("authToken").First());
            }
            return null;
        }
        public static Tok getAdminToken(HttpRequestMessage request)
        {
            if (request.Headers.Contains("adminToken"))
            {
                return JsonConvert.DeserializeObject<Tok>(request.Headers.GetValues("adminToken").First());
            }
            return null;
        }
        public static int getLoginId(HttpRequestMessage request)
        {
            Regex r = new Regex(@"\A/api/logins/(0|([1-9]+[0-9]*))\z");
            var m = r.Match(request.RequestUri.AbsolutePath);
            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }
            return -1;
        }
        public static int getUserId(HttpRequestMessage request)
        {
            Regex r = new Regex(@"\A/api/users/(0|([1-9]+[0-9]*))\z");
            var m = r.Match(request.RequestUri.AbsolutePath);
            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }
            return -1;
        }
        public static int getMatchId(HttpRequestMessage request)
        {
            Regex r = new Regex(@"\A/api/matches/(0|([1-9]+[0-9]*))\z");
            var m = r.Match(request.RequestUri.AbsolutePath);
            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }
            r = new Regex(@"\A/api/matches/(0|([1-9]+[0-9]*))/predictions\z");
            m = r.Match(request.RequestUri.AbsolutePath);
            if (m.Success)
            {
                return int.Parse(m.Groups[1].Value);
            }
            return -1;
        }
    }
}