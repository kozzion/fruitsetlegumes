using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.DataImport
{
    public class FeaturePage
    {
        public string Name { get; }
        public double[] Features { get; }
        
        public FeaturePage(string name, double [] features)
        {
            Name = name;
            Features = features;
        }
    }
}
