using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Boxing.App_Start
{
    public class Client : HttpClient
    {
        public Client() : base() {
            BaseAddress = new Uri("http://localhost:50777/");
            DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}