using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Core.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = null) : base(message) { }
    }
}
