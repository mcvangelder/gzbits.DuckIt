using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gzbits.DuckIt
{
    public class DuckItException : Exception
    {
        public DuckItException(string? message) : base(message)
        {
        }

        public DuckItException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
