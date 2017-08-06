using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public class MethodIntersect : IMethod
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
