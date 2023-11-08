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
        internal Chromosome chromosome = new Chromosome();
        protected int rectionTimeMilliseconds;

        protected Living(int rectionTimeMilliseconds)
        {
            this.rectionTimeMilliseconds = rectionTimeMilliseconds;
            reactionTimer = new Timer(RunningReactions, null, 100, rectionTimeMilliseconds);
        }
        protected Living() : this(1000)
        {
        }

        //Physical parameters
        protected Vector3 position = Vector3.Zero;
        public int X => (int)position.X;
        public int Y => (int)position.Y;
        public int Z => (int)position.Z;
        public double Distance(IPlacable obj)
        {
            return Math.Sqrt(Math.Pow(X - obj.X, 2) + Math.Pow(Y - obj.Y, 2) + Math.Pow(Z - obj.Z, 2));
        }

        //Reproduction
        public abstract Living Reproduction(params Living[] others);


        //Reaction
        protected Dictionary<Type, Action<IPlacable>> stimulusReactions = new Dictionary<Type, Action<IPlacable>>();

        protected Dictionary<string, Action<int>> conditionReactions = new Dictionary<string, Action<int>>();
        private Dictionary<string, Action<int>> currentlyRunningConditionReactions = new Dictionary<string, Action<int>>();
        private readonly Timer reactionTimer;

        private void RunningReactions(object? state)
        {
            var actions = currentlyRunningConditionReactions.Values.ToArray();
            foreach (var action in actions)
            {
                action.Invoke(rectionTimeMilliseconds);
            }
        }
        protected void StartConditionReaction(string condition)
        {
            if (conditionReactions.ContainsKey(condition))
            {
                if (currentlyRunningConditionReactions.ContainsKey(condition))
                {
                    currentlyRunningConditionReactions[condition] = conditionReactions[condition];
                }
                else
                {
                    currentlyRunningConditionReactions.Add(condition, conditionReactions[condition]);
                }
            }
        }
        protected void StopConditionReaction(string condition)
        {
            currentlyRunningConditionReactions.Remove(condition);
        }


        //Consume
        protected abstract void Consume(IEatable food);
    }
}