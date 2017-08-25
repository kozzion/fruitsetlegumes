using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FruitsetlegumesCL.Method
{
    public class MethodIntersect : IModelBuilder
    {
        public AModel Create(IList<string> labels, IList<IList<TokenPage>> data)
        {
            if (labels.Count != data.Count)
            {
                throw new InvalidOperationException();
            }

            List<HashSet<string>> list = new List<HashSet<string>>();
            foreach (var item in data)
            {
                list.Add(new HashSet<string>(item.SelectMany(l => l.Map.Keys)));
            }

            return new ModelIntersect(labels, list);
        }
    }
}
