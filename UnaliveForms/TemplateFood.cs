using NNTestFrame.MainComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.UnalifeForms
{
    public class TemplateFood : IEatable
    {
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
                if(nearest is null || distance > Distance(placable))
                {
                    nearest = placable;
                    distance = Distance(placable);
                }
            }
            return nearest;
        }

        public Vector3 Position { get { return position; } }

        public Dictionary<string, float> Nutrition => new Dictionary<string, float>();

        public TemplateFood(Vector3 position)
        {
            this.position = position;
        }
    }
}
