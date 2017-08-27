using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.DataImport
{
    public class DataSetTokens
    {

        private const string CacheDirectory = "cache_dataset_tokens";
        private static readonly string CachePath = Path.Combine(Environment.CurrentDirectory, CacheDirectory);

        public string Name { get;  }
        public List<TokenPage> TokenPages { get; }

        private DataSetTokens(string name, List<TokenPage> tokenPages)
        {
            Name = name;
            TokenPages = tokenPages;
        }

        public DataSetTokens Create(ImportWiki import, String name, List<string> urlList)
        {
            var path = CreatePath(name);

            DataSetTokens dataSet;
            if (File.Exists(path))
            {
                dataSet = JsonConvert.DeserializeObject<DataSetTokens>(File.ReadAllText(path));
            }
            else
            {
                dataSet = new DataSetTokens(name, urlList.Select(import.Import).ToList());
                File.WriteAllText(path, JsonConvert.SerializeObject(dataSet));
            }
            return dataSet;
        }

        public string CreatePath(string name)
        {
            return Path.Combine(CacheDirectory, name + ".json");            
        }

    }
}
