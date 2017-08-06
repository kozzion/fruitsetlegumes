using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FruitsetlegumesCL.DataImport
{
    public static class Tokenizer
    {
        public static IEnumerable<string> GetTokens(string html)
        {
            List<string> result_0 = ParseTag("<p>", "</p>", html);
            List<string> result_1 = ExcludeTag("<", ">", result_0);
            List<string> result_2 = ExcludeTag("[", "]", result_1);

            var re = new Regex("\\w+", RegexOptions.Compiled);

            var words = new List<string>();

            foreach (var line in result_2)
            {
                var matches = re.Matches(line);

                foreach (Match match in matches)
                {
                    var word = match.Value;
                    var normalized = Normalize(word);
                    words.Add(normalized);
                }
            }

            return words;
        }

        private static string Normalize(string word)
        {
            return word.ToLowerInvariant()
                .Replace('ä', 'a')
                .Replace('á', 'a')
                .Replace('à', 'a')
                .Replace('ö', 'o')
                .Replace('ó', 'o')
                .Replace('ò', 'o')
                .Replace('ç', 'c')
                .Replace('ë', 'e')
                .Replace('é', 'e')
                .Replace('è', 'e')
                .Replace('ï', 'i')
                .Replace('í', 'i')
                .Replace('ì', 'i')
                .Replace('ü', 'u')
                .Replace('ú', 'u')
                .Replace('ù', 'u');
        }

        private static List<string> ParseTag(string open_tag, string close_tag, string html)
        {
            List<string> result = new List<string>();
            int current_index = 0;
            while (true)
            {
                int open_index = html.IndexOf(open_tag, current_index);
                if (open_index == -1)
                {
                    break;
                }
                else
                {
                    open_index += open_tag.Length;
                    int close_index = html.IndexOf(close_tag, open_index);
                    if (close_index == -1)
                    {
                        break;
                    }
                    else
                    {
                        current_index = close_index;
                        result.Add(html.Substring(open_index, close_index - open_index));
                    }
                }
            }
            return result;
        }

        private static List<string> ExcludeTag(string open_tag, string close_tag, List<string> sections)
        {
            List<string> result = new List<string>();
            foreach (var section in sections)
            {
                result.AddRange(ExcludeTag(open_tag, close_tag, section));
            }
            return result;
        }

        private static List<string> ExcludeTag(string open_tag, string close_tag, string section)
        {
            List<string> result = new List<string>();
            int current_index = 0;
            while (true)
            {
                int open_index = section.IndexOf(open_tag, current_index);
                if (open_index == -1)
                {
                    // add remaining
                    result.Add(section.Substring(current_index));
                    break;
                }
                else
                {
                    // add current
                    result.Add(section.Substring(current_index, open_index - current_index));

                    int close_index = section.IndexOf(close_tag, open_index);
                    if (close_index == -1)
                    {
                        break;
                    }
                    else
                    {
                        current_index = close_index + close_tag.Length;
                    }
                }
            }
            return result;
        }
    }
}
