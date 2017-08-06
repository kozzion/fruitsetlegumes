using System.Collections.Generic;

namespace FruitsetlegumesCL.DataImport
{
    public struct TokenPage
    {
        public string Name { get; }
        public Dictionary<string, double> Map { get; }
        public IEnumerable<string> Tokens => Map.Keys;

        public TokenPage(string name, Dictionary<string, double> map)
        {
            Name = name;
            Map = map;
        }
    }
}
