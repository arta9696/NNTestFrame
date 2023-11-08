using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.MainComponents
{
    internal interface IPlacable
    {
        int X { get; }
        int Y { get; }
        int Z { get; }
        public double Distance(IPlacable obj);
    }
}
