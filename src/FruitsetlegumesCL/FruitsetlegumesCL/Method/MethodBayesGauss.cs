using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitsetlegumesCL.Method
{
    public class MethodBayesGauss : IModelBuilder
    {
        private double _epsilon;

        public MethodBayesGauss(double epsilon)
        {
            _epsilon = epsilon;
        }

        public AModel Create(IList<string> labels, IList<IList<TokenPage>> data)
        {
            if (labels.Count != data.Count)
            {
                throw new InvalidOperationException();
            }

            var estimates = new List<Dictionary<string, GaussEstimate>>();

            foreach (var typeClassPages in data)
            {
                var estimators = new Dictionary<string, GaussEstimator>();

                foreach (var page in typeClassPages)
                {
                    foreach (var pair in page.Map)
                    {
                        var word = pair.Key;
                        var value = pair.Value;

                        if (!estimators.TryGetValue(word, out var estimator))
                        {
                            estimator = new GaussEstimator();
                            estimators.Add(word, estimator);
                        }

                        estimator.AddValue(value);
                    }
                }

                var estimate = estimators.ToDictionary(pair => pair.Key, pair => pair.Value.GetEstimate());
                estimates.Add(estimate);
            }

            var totalCount = data.Sum(d => d.Count);
            var priors = data.Select(d => d.Count / (double)totalCount).ToList();

            return new ModelBayesGauss(labels, _epsilon, priors, estimates);
        }
    }
}