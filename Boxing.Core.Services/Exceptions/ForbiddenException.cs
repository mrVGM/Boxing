using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Services.Exceptions
{
    class ForbiddenException : Exception
    {
        public ForbiddenException(string message = null) : base(message) { }
    }
}
