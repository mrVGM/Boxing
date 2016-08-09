using Boxing.Contracts.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Boxing.Controllers
{
    public class LoginsController : Controller
    {
        private readonly HttpClient client;

        private class Token
        {
            public int id { get; set; }
            public string authToken { get; set; }
            public string adminToken { get; set; }
        }

        public LoginsController(HttpClient _client)
        {
            client = _client;
        }


        // GET: Logins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Logins/Create
        [HttpPost]
        public ActionResult Create(User formData)
        {
            try
            {
                string body = JsonConvert.SerializeObject(formData);

                HttpResponseMessage response = client.PostAsJsonAsync("/api/logins", formData).Result;

                //HttpResponseMessage response = client.PostAsync("/api/logins", new StringContent(body, Encoding.UTF8, "application/json")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return View("CreateFailed");
                }

                Token t = response.Content.ReadAsAsync<Token>().Result;

                if (t.authToken != null)
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie("authToken", t.authToken));
                    HttpCookie c = new HttpCookie("adminToken");
                    c.Expires = new DateTime(1970, 1, 1);
                    Response.Cookies.Add(c);
                }
                else if (t.adminToken != null)
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie("adminToken", t.adminToken));
                    HttpCookie c = new HttpCookie("authToken");
                    c.Expires = new DateTime(1970, 1, 1);
                    Response.Cookies.Add(c);
                }

                HttpContext.Response.Cookies.Add(new HttpCookie("id", t.id.ToString()));

                
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int id)
        {
            var idCookie = Request.Cookies.Get("id");
            var authTokenCookie = Request.Cookies.Get("authToken");
            var adminTokenCookie = Request.Cookies.Get("adminToken");

            if (authTokenCookie != null || adminTokenCookie != null)
            {
                if (adminTokenCookie != null)
                {
                    authTokenCookie = adminTokenCookie;
                }

                var authToken = JsonConvert.SerializeObject(new { login = int.Parse(idCookie.Value), token = authTokenCookie.Value });
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete, System.String.Format("/api/logins/{0}", id));
                req.Headers.Add("authToken", authToken);
                var re = client.SendAsync(req).Result;
            }
            
            HttpCookie c = new HttpCookie("authToken");
            c.Expires = new DateTime(1970, 1, 1);
            HttpContext.Response.Cookies.Add(c);
            c = new HttpCookie("id");
            c.Expires = new DateTime(1970, 1, 1);
            HttpContext.Response.Cookies.Add(c);
            c = new HttpCookie("adminToken");
            c.Expires = new DateTime(1970, 1, 1);
            HttpContext.Response.Cookies.Add(c);
            return RedirectToAction("Index", "Home");
        }
    }
}
