using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.MainComponents
{
    public interface IPlacable
    {
        int X { get; }
        int Y { get; }
        int Z { get; }
        public float Distance(IPlacable obj);
        public IPlacable FindNearest(IPlacable[] placables);
    }
}
