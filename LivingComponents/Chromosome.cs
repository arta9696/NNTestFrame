using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.LivingComponents
{
    internal class Chromosome
    {
        public double[] Chromo { get; }

        public Chromosome(params double[] chromodata)
        {
            Chromo = chromodata;
        }
    }
}
