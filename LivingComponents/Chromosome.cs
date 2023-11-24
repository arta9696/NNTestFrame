using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.LivingComponents
{
    public class Chromosome
    {
        public float[] Chromo { get; }

        public Chromosome(params float[] chromodata)
        {
            Chromo = chromodata;
        }
    }
}
