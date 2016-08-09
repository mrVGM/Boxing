using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex r = new Regex(@"\A/api/logins/(0|([1-9]+[0-9]*))\z");

            var a = r.Match("/api/logins/01");

        }
    }
}
