using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FruitsetlegumesCL.DataImport
{
    public struct TokenPage
    {
        public string Name { get; }
        public Dictionary<string, double> Map { get; }

        [JsonIgnore]
        public IEnumerable<string> Tokens => Map.Keys;

        public TokenPage(string name, Dictionary<string, double> map)
        {
            Name = name;
            Map = map;
        }

        internal FeaturePage ToFeaturePage(List<string> selectedTokens)
        {
            var map = Map;
            var features = selectedTokens.Select(token => map[token]).ToArray();
            return new FeaturePage(Name, features);
        }
    }
}
