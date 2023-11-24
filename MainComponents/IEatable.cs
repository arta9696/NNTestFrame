using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.MainComponents
{
    public interface IEatable: IPlacable
    {
        public Dictionary<string, float> Nutrition { get; }
    }
}
