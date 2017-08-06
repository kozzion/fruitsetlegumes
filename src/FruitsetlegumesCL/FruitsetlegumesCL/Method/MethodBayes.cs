using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public class MethodBayes : IMethod
    {
        public AModel Create(IList<string> labels, IList<IList<TokenPage>> data)
        {
            if (labels.Count != data.Count)
            {
                throw new InvalidOperationException();
            }



            IList<Dictionary<string, double>> logLikelyhoods = new List<Dictionary<string, double>>();
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

            var priors = Enumerable.Repeat(0.0, data.Count).ToList(); //TODO could use priors

            return new ModelBayes(labels, priors, logLikelyhoods);
        }
    }
}