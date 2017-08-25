using FruitsetlegumesCL.DataImport;
using FruitsetlegumesCL.Method;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FruitsetlegumesCL
{
    public static class Program
    {
        private const string fruit = "fruit";
        private const string vegetable = "vegetable";

        static void Main(string[] args)
        {
            const string fruitPage = "https://en.wikipedia.org/wiki/List_of_culinary_fruits";
            const string vegetablePage = "https://en.wikipedia.org/wiki/List_of_vegetables";
            const string cocktailPage = "https://en.wikipedia.org/wiki/List_of_cocktails";

            var import = new ImportWiki();
            var fruit_tokens = import.ImportRecursive(fruitPage);
            var vegetable_tokens = import.ImportRecursive(vegetablePage);
            var cocktailTokens = import.ImportRecursive(cocktailPage);

            var fruitExpectations = fruit_tokens.Select(page => new TokenPageExpectation(page, fruit));
            var vegetableExpectations = vegetable_tokens.Select(page => new TokenPageExpectation(page, vegetable));
            var allExpectations = fruitExpectations.Concat(vegetableExpectations);

            var label = new string[] { fruit, vegetable };
            var data = new IList<TokenPage>[] { fruit_tokens, vegetable_tokens };

            RunModel(new MethodIntersect(), label, data, allExpectations);
            RunModel(new MethodBayes(), label, data, allExpectations);
            var bayesGuass = RunModel(new MethodBayesGauss(1e-3), label, data, allExpectations);

            using (var writer = new StreamWriter("cocktails.csv"))
            {
                WritePageInfos(writer, cocktailTokens, bayesGuass);
            }

            Console.ReadLine();
        }

        private static AModel RunModel(
            IModelBuilder method,
            string[] labels,
            IList<TokenPage>[] data,
            IEnumerable<TokenPageExpectation> expectations)
        {
            var model = method.Create(labels, data);
            var matrix = model.Test(expectations);

            Console.WriteLine($"{"",-15} {fruit,15} {vegetable,15}");
            Console.WriteLine($"{fruit,-15} {matrix[0, 0],15} {matrix[0, 1],15}");
            Console.WriteLine($"{vegetable,-15} {matrix[1, 0],15} {matrix[1, 1],15}");
            Console.WriteLine();

            return model;
        }

        private static void WritePageInfos(TextWriter writer, IEnumerable<TokenPage> pages, AModel model)
        {
            writer.WriteLine("Page;Label");

            foreach (var page in pages)
            {
                PrintPageInfo(writer, page, model);
            }
        }

        private static void PrintPageInfo(TextWriter writer, TokenPage page, AModel model)
        {
            var name = page.Name;
            var scores = model.GetScores(page);

            var maxScore = scores.Max(s => s.Score);
            var label = scores.First(s => s.Score == maxScore).Type;

            writer.WriteLine($"{name};{label}");
        }
    }
}
