using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitsetlegumesCL.Method
{
    public class GaussEstimator
    {
        private List<double> _values = new List<double>();

        public void AddValue(double value)
        {
            _values.Add(value);
        }

        public GaussEstimate GetEstimate()
        {
            var mean = _values.Sum() / _values.Count;
            var stdDev = _values.Sum(v => Math.Pow(v - mean, 2.0)) / _values.Count;
            return new GaussEstimate { Mean = mean, StandardDeviation = stdDev };
        }
    }
}
