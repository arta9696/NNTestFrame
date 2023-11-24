using NNTestFrame.LivingComponents;
using NNTestFrame.MainComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.LiveForms
{
    public class TemplateLife : Living
    {
        static Random random = new Random();

        float satiety = 10f;
        enum Decisions { walk, eat, sleep }
        Decisions currentDecision = Decisions.sleep;
        Dictionary<string, object> objects_I_see = new Dictionary<string, object>();
        public TemplateLife() : base(new Chromosome(
            randomRange(1, 5), 
            randomRange(16, 24), 
            randomRange(1,5)
        ))
        {
        }

        private static float randomRange(double rangeMin, double rangeMax)
        {
            return (float)(rangeMin + (rangeMax - rangeMin) * random.NextDouble());
        }

        public override void Consume(IEatable food)
        {
            food.Nutrition.TryGetValue("Nutrition", out float nutriens);
            satiety += nutriens;
        }

        public void Hunger()
        {
            satiety -= chromosome.Chromo[2];
        }
        public void Move()
        {

        }

        public override void ISee(Dictionary<string, object> objects_dictionary)
        {
            objects_I_see = objects_dictionary;
            //Place AI here

            //placeholder
            if(satiety+5f> chromosome.Chromo[1])
            {
                currentDecision = Decisions.sleep;
            }
            if(satiety - 5f < chromosome.Chromo[0])
            {
                if (FindNearest((IEatable[])objects_I_see["Food"]).Distance(this) < 25)
                {
                    currentDecision = Decisions.eat;
                }
                else
                {
                    currentDecision = Decisions.walk;
                }
            }
        }

        public override Dictionary<string, object> IThink()
        {
            if (satiety < chromosome.Chromo[0])
            {
                throw new IDie("Hungered");
            }
            if(satiety > chromosome.Chromo[1])
            {
                throw new IDie("Overeat");
            }

            var toRet = new Dictionary<string, object>() {
                { "IThink", currentDecision.ToString() }
            };

            if(currentDecision == Decisions.walk)
            {
                toRet.Add("Direction", new Vector3());
            }
            if(currentDecision == Decisions.eat)
            {
                toRet.Add("Food", FindNearest((IEatable[])objects_I_see["Food"]));
            }

            return toRet;
        }

        public override Living Reproduction(params Living[] others)
        {
            throw new NotImplementedException();
        }
    }
}
