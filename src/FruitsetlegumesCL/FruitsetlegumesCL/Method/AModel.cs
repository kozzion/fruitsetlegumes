using FruitsetlegumesCL.DataImport;
using System.Collections.Generic;
using System.Linq;
using static FruitsetlegumesCL.Method.ModelIntersect;

namespace FruitsetlegumesCL.Method
{

    public struct TypeScore
    {
        public string Type;
        public double Score;

        public TypeScore(string type, double score)
        {
            Type = type;
            Score = score;
        }
    }


    public abstract class AModel
    {
        public IList<string> Labels;


        protected AModel(IList<string> labels)
        {
            Labels = labels;
        }

        public int[,] Test(IEnumerable<TokenPageExpectation> expectations)
        {
            int[,] confusionMatrix = new int[Labels.Count, Labels.Count];

            foreach (var expectation in expectations)
            {
                var item = expectation.Page;
                var expectedLabel = expectation.Expectation;
                List<TypeScore> score = GetScores(item);
                var maxScore = score.Max(s => s.Score);
                var actualLabel = score.First(s => s.Score == maxScore).Type;

                int actualLabelIndex = Labels.IndexOf(actualLabel);
                int expectedLabelIndex = Labels.IndexOf(expectedLabel);
                confusionMatrix[expectedLabelIndex, actualLabelIndex]++;
            }
            return confusionMatrix;
        }

        public int StringToIndex(string label)
        {
            return Labels.IndexOf(label);
        }


        public abstract List<TypeScore> GetScores(TokenPage page);

    }
}