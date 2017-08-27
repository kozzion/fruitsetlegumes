using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public class EvaluationResult
    {
        public double HitRate { get; }
        public List<string> Labels {get; set;}
        public int[,] _confusionMatrix;

        public EvaluationResult(List<string> labels, int [,] confusionMatrix)
        {
            Labels = labels;
            _confusionMatrix = confusionMatrix;
        }
    }
}
