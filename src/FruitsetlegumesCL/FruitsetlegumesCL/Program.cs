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

        private const string fruitPage = "https://en.wikipedia.org/wiki/List_of_culinary_fruits";
        private const string vegetablePage = "https://en.wikipedia.org/wiki/List_of_vegetables";
        private const string cocktailPage = "https://en.wikipedia.org/wiki/List_of_cocktails";

        private const int fruitTrainingSetSize = 500;
        private const int vegetableTrainingSetSize = 100;

        private static double ScoreResult(int expectedCount, int[,] matrix)
        {
            Console.WriteLine($"{"",-15} {fruit,15} {vegetable,15}");
            Console.WriteLine($"{fruit,-15} {matrix[0, 0],15} {matrix[0, 1],15}");
            Console.WriteLine($"{vegetable,-15} {matrix[1, 0],15} {matrix[1, 1],15}");
            Console.WriteLine();

            var actualCount = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                actualCount += matrix[i, i];
            }

            return (double)actualCount / expectedCount;
        }

        internal static void Main(string[] args)
        {
            var import = new ImportWiki();
            var fruitTokens = import.ImportRecursive(fruitPage);
            var vegetableTokens = import.ImportRecursive(vegetablePage);
            var cocktailTokens = import.ImportRecursive(cocktailPage);

            var expectedCount = fruitTokens.Count + vegetableTokens.Count;

            var fruitExpectations = fruitTokens.Select(page => new TokenPageExpectation(page, fruit));
            var vegetableExpectations = vegetableTokens.Select(page => new TokenPageExpectation(page, vegetable));
            var allExpectations = fruitExpectations.Concat(vegetableExpectations);

            var label = new string[] { fruit, vegetable };
            var data = new IList<TokenPage>[] { fruitTokens, vegetableTokens };

            var trainingData = new IList<TokenPage>[] { fruitTokens.Take(fruitTrainingSetSize).ToList(), vegetableTokens.Take(vegetableTrainingSetSize).ToList() };
            var fruitTestingExpectations = fruitExpectations.Skip(fruitTrainingSetSize);
            var vegetableTestingExpectations = vegetableExpectations.Skip(vegetableTrainingSetSize);
            var testingExpectations = fruitTestingExpectations.Concat(vegetableTestingExpectations).ToArray();


            //RunModel(new MethodIntersect(), label, data, allExpectations);
            //RunModel(new MethodBayes(), label, data, allExpectations);

            TestModels(expectedCount, label, trainingData, testingExpectations);

            //var bayesGuass = RunModel(new MethodBayesGauss(1e-3), label, data, allExpectations);

            //using (var writer = new StreamWriter("cocktails.csv"))
            //{
            //    WritePageInfos(writer, cocktailTokens, bayesGuass);
            //}

            Console.ReadLine();
        }

        private static void TestModels(
            int expectedCount,
            string[] labels,
            IList<TokenPage>[] trainingData,
            IEnumerable<TokenPageExpectation> expectations)
        {
            Console.WriteLine("Testing best epsilon");

            var bestEpsilon = double.NaN;
            var bestScore = 0.0;

            for (var epsilon = 1e-11; epsilon < 1e-9; epsilon += 1e-11)
            {
                Console.WriteLine($"Epsilon: {epsilon}");
                var method = new MethodBayes(epsilon);
                var model = method.Create(labels, trainingData);
                var matrix = model.Test(expectations);

                var score = ScoreResult(expectedCount, matrix);
                Console.WriteLine($"Score: {score}");

                if (score > bestScore)
                {
                    bestScore = score;
                    bestEpsilon = epsilon;
                }
            }

            Console.WriteLine($"Best epsilon: {bestEpsilon}");
            Console.WriteLine($"Best score:   {bestScore}");
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
