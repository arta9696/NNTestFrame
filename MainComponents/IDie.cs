using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.MainComponents
{
    public class IDie : Exception
    {
        public IDie(string? message) : base(message)
        {
        }

        public IDie(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
