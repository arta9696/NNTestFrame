using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NNTestFrame.LivingComponents;

namespace NNTestFrame.MainComponents
{
    public abstract class Living : IPlacable
    {
        //Behavior
        protected Chromosome chromosome;

        public Living(Chromosome chromosome)
        {
            this.chromosome = chromosome;
        }

        //Physical parameters
        protected Vector3 position = Vector3.Zero;
        public int X => (int)position.X;
        public int Y => (int)position.Y;
        public int Z => (int)position.Z;
        public float Distance(IPlacable obj)
        {
            return (float)Math.Sqrt(Math.Pow(X - obj.X, 2) + Math.Pow(Y - obj.Y, 2) + Math.Pow(Z - obj.Z, 2));
        }
        public IPlacable FindNearest(IPlacable[] placables)
        {
            IPlacable nearest = null;
            float distance = 0;
            foreach (var placable in placables)
            {
                if (nearest is null || distance > Distance(placable))
                {
                    nearest = placable;
                    distance = Distance(placable);
                }
            }
            return nearest;
        }

        //Reproduction
        public abstract Living Reproduction(params Living[] others);

        //Reaction
        public abstract void ISee(Dictionary<string, object> objects_dictionary);
        public abstract Dictionary<string, object> IThink();

        //Consume
        public abstract void Consume(IEatable food);
    }
}