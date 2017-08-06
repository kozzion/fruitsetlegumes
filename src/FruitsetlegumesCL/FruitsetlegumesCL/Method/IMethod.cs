using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public interface IMethod
    {
        AModel Create(IList<string> labels, IList<IList<TokenPage>> data);
    }
}
