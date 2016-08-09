using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Boxing.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient client;
        public UsersController(HttpClient _client)
        {
            client = _client;
        }
        // GET: Users
        public ActionResult Index()
        {
            if (Request.Cookies.Get("id") == null)
            {
                return RedirectToAction("Create", "Logins");
            }

            ViewBag.id = Request.Cookies.Get("id").Value;
            
            if (Request.Cookies.Get("adminToken") != null)
            {
                return View("IndexAdmin");
            }
            if (Request.Cookies.Get("authToken") != null)
            {
                return View("Index");
            }
            
            return RedirectToAction("Create", "Logins");
        }

        public ActionResult Ajax(int skip, int take, string sortBy, string order)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/users?skip={0}&take={1}&sortBy={2}&order={3}", skip, take, sortBy, order));
            req = TokenHeader.addTokenHeader(Request.Cookies, req);

            var response = client.SendAsync(req).Result;

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<User> users = response.Content.ReadAsAsync<IEnumerable<User>>().Result;
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(response.StatusCode);
            }
        }

     
        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(User collection)
        {
            try
            {
                var response = client.PostAsJsonAsync("/api/users", collection).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return View("CreateFailed");
                }

                return RedirectToAction("Create", "Logins");
            }
            catch
            {
                return View();
            }
        }

     
        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, string.Format("/api/users/{0}",id));
            TokenHeader.addTokenHeader(Request.Cookies, message);
            var response = client.SendAsync(message).Result;

            return new HttpStatusCodeResult(response.StatusCode);
        }
    }
}
