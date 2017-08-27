using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.DataImport
{
    public class DataSetFeatures
    {
        private const string CacheDirectory = "cache_dataset_features";
        private static readonly string CachePath = Path.Combine(Environment.CurrentDirectory, CacheDirectory);

        public string Name { get; }
        public List<FeaturePage> FeaturePages { get; }
        public List<string> SelectedTokenTypes { get; }

        private DataSetFeatures(string name, List<FeaturePage> featurePages, List<string> selectedTokenTypes)
        {
            Name = name;
            FeaturePages = featurePages;
            SelectedTokenTypes = selectedTokenTypes;
        }

        public DataSetFeatures Create(string name, ImportWiki import, List<string> selectedTokenTypes, List<string> urlList)
        {
            var path = CreatePath(name);

            DataSetFeatures dataSet;

            if (File.Exists(path))
            {
                dataSet = JsonConvert.DeserializeObject<DataSetFeatures>(File.ReadAllText(path));
            }
            else
            {
                var featurePages = urlList.Select(url => import.Import(url, selectedTokenTypes)).ToList();
                dataSet = new DataSetFeatures(name, featurePages, selectedTokenTypes);
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
