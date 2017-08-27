using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitsetlegumesCL.Method
{
    public class MethodBayes : IModelBuilder
    {
        private double _epsilon;

        public MethodBayes(double epsilon)
        {
            _epsilon = epsilon;
        }

        public AModel Create(IList<string> labels, IList<IList<TokenPage>> data)
        {
            if (labels.Count != data.Count)
            {
                throw new InvalidOperationException();
            }

            var logLikelyhoods = new List<Dictionary<string, double>>();

            foreach (var typeClass in data)
            {
                var total = 0.0;
                var tokenCounts = new Dictionary<string, int>();
                
                foreach (var page in typeClass)
                {
                    foreach (var token in page.Tokens)
                    {
                        total++;
                        int tokenCount;
                        tokenCounts.TryGetValue(token, out tokenCount);
                        tokenCounts[token] = tokenCount + 1;
                    }
                }

                var logLikelyhood = tokenCounts.ToDictionary(pair => pair.Key, pair => Math.Log(pair.Value / total));
                logLikelyhoods.Add(logLikelyhood);
            }

            var totalCount = data.Sum(d => d.Count);
            var priors = data.Select(d => d.Count / (double)totalCount).ToList();

            return new ModelBayes(labels, _epsilon, priors, logLikelyhoods);
        }
    }
}