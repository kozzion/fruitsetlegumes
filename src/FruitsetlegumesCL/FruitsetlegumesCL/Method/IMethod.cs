using FruitsetlegumesCL.DataImport;
using System.Collections.Generic;

namespace FruitsetlegumesCL.Method
{
    public interface IModelBuilder
    {
        AModel Create(IList<string> labels, IList<IList<TokenPage>> data);
    }
}
