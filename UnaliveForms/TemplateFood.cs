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
        public double Distance(IPlacable obj)
        {
            return Math.Sqrt(Math.Pow(X - obj.X, 2) + Math.Pow(Y - obj.Y, 2) + Math.Pow(Z - obj.Z, 2));
        }
        public Vector3 Position { get { return position; } }

        public Dictionary<string, double> Nutrition => new Dictionary<string, double>();

        public TemplateFood(Vector3 position, double callories)
        {
            this.position = position;
            Nutrition.Add("Callories", callories);
        }
    }
}
