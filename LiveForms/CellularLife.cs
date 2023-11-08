using NNTestFrame.MainComponents;
using NNTestFrame.UnalifeForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.LifeForms
{
    public class CellularLife : Living, IMovable
    {
        TemplateFood? nearestFood;
        double cellRadius = 5;
        double visibilityRadius = 50;
        double speed = 1d;
        public CellularLife():base()
        {
            stimulusReactions.Add(typeof(TemplateFood), FoodReaction);
            /*!!!*///stimulusReactions.Add(typeof(TemplateFood), NearFoodReaction); // <- Create NotADictionary that can take multiple KeyValuePairs with the same Key. Try to make it same formate as Dictionary

            conditionReactions.Add("FoodMove", (deltaTime) => Move(Vector3.Normalize(nearestFood.Position - position) * (float)speed * deltaTime / 1000f));
            conditionReactions.Add("Log", (deltaTime) => { Console.WriteLine($"{position} {cellRadius} {visibilityRadius}"); });
            StartConditionReaction("Log");
        }

        public void GrowCell(double deltaSize)
        {
            cellRadius += deltaSize;
            visibilityRadius += deltaSize*10;
        }

        public void Move(Vector3 deltaPosition)
        {
            position += deltaPosition;
        }

        public override Living Reproduction(params Living[] others)
        {
            var temp = new CellularLife();
            temp.position = position;
            return temp;
        }

        protected override void Consume(IEatable food)
        {
            GrowCell(food.Nutrition["Callories"]);
        }

        private void FoodReaction(IPlacable food)
        {
            if(!(food is TemplateFood tFood))
            {
                return;
            }
            if(!(nearestFood == null) && Distance(nearestFood) < Distance(tFood))
            {
                return;
            }
            nearestFood = tFood;
            StartConditionReaction("FoodMove");
        }

        private void NearFoodReaction(IPlacable food)
        {
            if (!(food is TemplateFood tFood))
            {
                return;
            }
            if (Distance(tFood) < cellRadius)
            {
                Consume(tFood);
            }
        }
    }
}
