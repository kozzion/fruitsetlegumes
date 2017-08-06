using FruitsetlegumesCL.DataImport;

namespace FruitsetlegumesCL.Method
{
    public struct TokenPageExpectation
    {
        public TokenPage Page { get; }
        public string Expectation { get; }

        public TokenPageExpectation(TokenPage page, string expectation)
        {
            Page = page;
            Expectation = expectation;
        }
    }
}
