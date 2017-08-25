using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;

namespace FruitsetlegumesCL.Method
{
    public class ModelBayes : AModel
    {
        private IList<double> _prior;
        private IList<Dictionary<string,double>> _logLikelyhoods;

        public ModelBayes(IList<string> labels, IList<double> prior, IList<Dictionary<string, double>> logLikelyhoods)
            : base(labels)
        {
            _prior = prior;
            _logLikelyhoods = logLikelyhoods;
        }

        public override List<TypeScore> GetScores(TokenPage page)
        {
            List<TypeScore> list = new List<TypeScore>();
            for (int index = 0; index < Labels.Count; index++)
            {
                list.Add(new TypeScore(Labels[index], CreateScore(_prior[index],_logLikelyhoods[index], page.Tokens)));
            }
            return list;
        }

        public double CreateScore(double prior, Dictionary<string, double> logLikelyhoods, IEnumerable<string> tokens)
        {
            double logLikelihood = 0;
            var epsilon = Math.Log(1.0 / logLikelyhoods.Keys.Count);

            foreach (var item in tokens)
            {
                if (logLikelyhoods.TryGetValue(item, out double likelyhood))
                {
                    logLikelihood += likelyhood;
                }
                else
                {
                    logLikelihood += epsilon;
                }
            }
            return logLikelihood;
        }

    }
}
