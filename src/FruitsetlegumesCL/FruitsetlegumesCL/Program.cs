using FruitsetlegumesCL.DataImport;
using FruitsetlegumesCL.Method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL
{
    class Program
    {
        static void Main(string[] args)
        {
            string fruit_page = "https://en.wikipedia.org/wiki/List_of_culinary_fruits";
            string vegetable_page = "https://en.wikipedia.org/wiki/List_of_vegetables";


            ImportWiki import = new ImportWiki();
            var fruit_tokens = import.ImportRecursive(fruit_page);
            var vegetable_tokens = import.ImportRecursive(vegetable_page);

            var fruitExpectations = fruit_tokens.Select(page => new TokenPageExpectation(page, "fruit"));
            var vegetableExpectations = vegetable_tokens.Select(page => new TokenPageExpectation(page, "vegetable"));
            var allExpectations = fruitExpectations.Concat(vegetableExpectations);

            var label = new string[] { "fruit", "vegetable" };
            var data = new IList<TokenPage>[] { fruit_tokens, vegetable_tokens };

            AModel model_0 = new MethodIntersect().Create(label, data);
            int [,] matrix_0  = model_0.Test(allExpectations);
            Console.WriteLine($"{"",-15} {"fuit",15} {"vegetable",15}");
            Console.WriteLine($"{"fuit",-15} {matrix_0[0,0],15} {matrix_0[0,1],15}");
            Console.WriteLine($"{"vegetable",-15} {matrix_0[1,0],15} {matrix_0[1, 1],15}");
            Console.WriteLine();


            AModel model_1 = new MethodBayes().Create(label, data);
            int[,] matrix_1 = model_1.Test(allExpectations);
            Console.WriteLine($"{"",-15} {"fuit",15} {"vegetable",15}");
            Console.WriteLine($"{"fuit",-15} {matrix_1[0, 0],15} {matrix_1[0, 1],15}");
            Console.WriteLine($"{"vegetable",-15} {matrix_1[1, 0],15} {matrix_1[1, 1],15}");
            //Console.WriteLine($"{"Name",-60}{"Fruit Score",15}{"Veg Score",15} {"Categorization",-15}{"Actual",-20}");

            //foreach (var page in fruit_tokens)
            //{
            //    PrintPageInfo(page, fruit_set, vegetable_set, "fruit");
            //}

            //foreach (var page in vegetable_tokens)
            //{
            //    PrintPageInfo(page, fruit_set, vegetable_set, "vegetable");
            //}

            Console.ReadLine();
        }

        //private static void PrintPageInfo(TokenPage page, HashSet<string> fruits, HashSet<string> veggies, string expected)
        //{
        //    var name = page.Name;
        //    var fruitScore = CreateScore(fruits, page.Map.Keys);
        //    var vegScore = CreateScore(veggies, page.Map.Keys);
        //    var category = fruitScore > vegScore ? "fruit" : vegScore > fruitScore ? "vegetable" : "unknown";
        //    Console.WriteLine($"{name,-60}{fruitScore,15}{vegScore,15} {category,-15}{expected,-20}");
        //}



    }
}
