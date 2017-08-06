using System.Collections.Generic;

namespace FruitsetlegumesCL.DataImport
{
    public struct HtmlPage
    {
        public string Name { get; }
        public string Html { get; }

        public HtmlPage(string name, string html)
        {
            Name = name;
            Html = html;
        }

        public TokenPage ToTokenPage()
        {
            var counts = new Dictionary<string, int>();
            var tokens = Tokenizer.GetTokens(Html);

            var total = 0.0;

            foreach (var token in tokens)
            {
                total++;

                int typeCount;
                counts.TryGetValue(token, out typeCount);
                counts[token] = typeCount + 1;
            }

            var map = new Dictionary<string, double>();

            foreach (var pair in counts)
            {
                map.Add(pair.Key, pair.Value / total);
            }

            return new TokenPage(Name, map);
        }
    }
}
