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
    public class MatchesController : Controller
    {
        private readonly HttpClient client;
        public MatchesController(HttpClient _client)
        {
            client = _client;
        }
        // GET: Matches
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
                return View("IndexUser");
            }
            return RedirectToAction("Create", "Logins");
        }

        
        // GET: Matches/Create
        public ActionResult Create()
        {
            if (Request.Cookies.Get("id") == null)
            {
                return RedirectToAction("Create", "Logins");
            }
            ViewBag.id = Request.Cookies.Get("id").Value;
            return View();
        }

        // POST: Matches/Create
        [HttpPost]
        public ActionResult Create(Match collection)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "/api/matches");
                message.Content = new StringContent(JsonConvert.SerializeObject(collection), Encoding.UTF8, "application/json");
                message = TokenHeader.addTokenHeader(Request.Cookies, message);

                var response = client.SendAsync(message).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Create", "Logins");
                }
                ViewBag.id = Request.Cookies.Get("id").Value;
                return View("CreateFailed");
            }
            catch
            {
                return View();
            }
        }

        // GET: Matches/Prediction/Id
        public ActionResult Prediction(int id)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, string.Format("api/matches/{0}/predictions", id));
            req = TokenHeader.addTokenHeader(Request.Cookies, req);
            var res = client.SendAsync(req).Result;
            if (res.IsSuccessStatusCode)
            {
                Match match = res.Content.ReadAsAsync<Match>().Result;
                return Json(match, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(res.StatusCode);
        }

        // POST: Matches/Prediction/Id
        [HttpPost]
        public ActionResult Prediction(int id, Match collection)
        {
            string message = JsonConvert.SerializeObject(collection);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, string.Format("api/matches/{0}/predictions", id));
            req.Content = new StringContent(message, Encoding.UTF8, "application/json");
            req = TokenHeader.addTokenHeader(Request.Cookies, req);
            var res = client.SendAsync(req).Result;
            if (res.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return new HttpStatusCodeResult(res.StatusCode);
        }
        
        [HttpPost]
        public ActionResult Winner(int id, Match collection)
        {
            collection.id = id;
            string message = JsonConvert.SerializeObject(collection);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Put, string.Format("api/matches", id));
            req.Content = new StringContent(message, Encoding.UTF8, "application/json");
            req = TokenHeader.addTokenHeader(Request.Cookies, req);
            var res = client.SendAsync(req).Result;
            if (res.IsSuccessStatusCode)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            
            return new HttpStatusCodeResult(res.StatusCode);
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int id)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete, string.Format("/api/matches/{0}", id));
            req = TokenHeader.addTokenHeader(Request.Cookies, req);
            var resp = client.SendAsync(req).Result;

            return new HttpStatusCodeResult(resp.StatusCode);
        }

        public ActionResult Ajax(int skip, int take, string searchString)
        {
            if (Request.Cookies.Get("adminToken") != null)
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, "/api/matches");
                message = TokenHeader.addTokenHeader(Request.Cookies, message);
                var resp = client.SendAsync(message).Result;
                if (resp.IsSuccessStatusCode)
                {
                    Match[] matches = resp.Content.ReadAsAsync<Match[]>().Result;

                    object[] res = new object[matches.Length];
                    for (int i = 0; i < matches.LongCount(); i++)
                    {
                        res[i] = new
                        {
                            id = matches[i].id,
                            boxer1 = matches[i].boxer1,
                            boxer2 = matches[i].boxer2,
                            place = matches[i].place,
                            dateOfMatch = matches[i].dateOfMatch.ToString(),
                            description = matches[i].description,
                            hasFinished = matches[i].hasFinished,
                            winner = matches[i].winner
                        };
                    }

                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                return new HttpUnauthorizedResult();
            }
            else if (Request.Cookies.Get("authToken") != null)
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/matches?skip={0}&take={1}&searchString={2}", skip, take, searchString));
                message = TokenHeader.addTokenHeader(Request.Cookies, message);
                var resp = client.SendAsync(message).Result;

                if (resp.IsSuccessStatusCode)
                {
                    Match[] matches = resp.Content.ReadAsAsync<Match[]>().Result;
                    object[] res = new object[matches.Length];
                    for (int i = 0; i < matches.LongCount(); i++)
                    {
                        res[i] = new
                        {
                            id = matches[i].id,
                            boxer1 = matches[i].boxer1,
                            boxer2 = matches[i].boxer2,
                            place = matches[i].place,
                            dateOfMatch = matches[i].dateOfMatch.ToString(),
                            description = matches[i].description,
                            hasFinished = matches[i].hasFinished,
                            winner = matches[i].winner
                        };
                    }
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                return new HttpUnauthorizedResult();
            }
            return new HttpUnauthorizedResult();
        }

    }
}
