using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public class ModelBayes : AModel
    {
        private IList<double> _Prior;
        private IList<Dictionary<string,double>> _LogLikelyhoods;

        public ModelBayes(IList<string> labels, IList<double> prior, IList<Dictionary<string, double>> logLikelyhoods)
            : base(labels)
        {
            _Prior = prior;
            _LogLikelyhoods = logLikelyhoods;
        }

        public override List<TypeScore> GetScores(TokenPage page)
        {
            List<TypeScore> list = new List<TypeScore>();
            for (int index = 0; index < Labels.Count; index++)
            {
                list.Add(new TypeScore(Labels[index], CreateScore(_Prior[index],_LogLikelyhoods[index], page.Tokens)));
            }
            return list;
        }

        public double CreateScore(double prior, Dictionary<string, double> logLikelyhoods, IEnumerable<string> tokens)
        {
            double logLikelihood = 0;
            var epsilon = Math.Log(1.0 / logLikelyhoods.Keys.Count);

            foreach (var item in tokens)
            {
                double likelyhood;

                if (logLikelyhoods.TryGetValue(item, out likelyhood))
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
