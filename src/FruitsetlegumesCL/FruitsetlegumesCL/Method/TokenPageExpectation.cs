using FruitsetlegumesCL.DataImport;

namespace FruitsetlegumesCL.Method
{
    public struct TokenPageExpectation
    {
        public TokenPage Page { get; }
        public string ExpectedCategory { get; }

        public TokenPageExpectation(TokenPage page, string expectedCategory)
        {
            Page = page;
            ExpectedCategory = expectedCategory;
        }
    }
}
