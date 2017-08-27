using System;
using System.Collections.Generic;
using FruitsetlegumesCL.DataImport;

namespace FruitsetlegumesCL.Method
{
    internal class ModelBayesGauss : AModel
    {
        private double _epsilonLn;
        private List<double> _priors;
        private List<Dictionary<string, GaussEstimate>> _estimates;

        public ModelBayesGauss(IList<string> labels, double epsilon, List<double> priors, List<Dictionary<string, GaussEstimate>> estimates)
            : base(labels)
        {
            _epsilonLn = Math.Log(epsilon);
            _priors = priors;
            _estimates = estimates;
        }

        public override List<TypeScore> GetScores(TokenPage page)
        {
            List<TypeScore> list = new List<TypeScore>();

            for (int index = 0; index < Labels.Count; index++)
            {
                list.Add(new TypeScore(Labels[index], CreateScore(_priors[index], _estimates[index], page.Map)));
            }

            return list;
        }

        public double CreateScore(double prior, Dictionary<string, GaussEstimate> estimates, Dictionary<string, double> map)
        {
            var logLikelihood = Math.Log(prior);

            foreach (var pair in map)
            {
                var token = pair.Key;
                var fraction = pair.Value;

                if (estimates.TryGetValue(token, out var estimate))
                {
                    var density = estimate.GetDensityLn(fraction);
                    logLikelihood += double.IsNaN(density) ? _epsilonLn : density;
                }
                else
                {
                    logLikelihood += _epsilonLn;
                }
            }

            return logLikelihood;
        }
    }
}