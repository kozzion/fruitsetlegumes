using MathNet.Numerics.Distributions;

namespace FruitsetlegumesCL.Method
{
    public struct GaussEstimate
    {
        public double StandardDeviation;
        public double Mean;

        public double GetDensityLn(double value)
        {
            return Normal.PDFLn(Mean, StandardDeviation, value);
        }

        public override string ToString()
        {
            return $"x̄={Mean},σ={StandardDeviation}";
        }
    }
}