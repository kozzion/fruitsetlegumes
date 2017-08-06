using FruitsetlegumesCL.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.Method
{
    public class ModelIntersect : AModel
    {
        private IList<HashSet<string>> _hashSets;

        public ModelIntersect(IList<string> labels, IList<HashSet<string>> hashSets)
            : base(labels)
        {
            _hashSets = hashSets;
        }


        public override List<TypeScore> GetScores(TokenPage page)
        {
            List<TypeScore> list = new List<TypeScore>();
            for (int index = 0; index < Labels.Count; index++)
            {
                list.Add(new TypeScore(Labels[index], CreateScore(_hashSets[index], page.Tokens)));
            }
            return list;
        }

        public int CreateScore(HashSet<string> hash_set, IEnumerable<string> tokens)
        {
            int score = 0;
            foreach (var item in tokens)
            {
                if (hash_set.Contains(item))
                {
                    score++;
                }
            }
            return score;
        }

    }
}
