using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FruitsetlegumesCL.DataImport
{
    public class ImportWiki
    {
        private const string map_path = @"D:\Projects\Fruitsetlegumes\backup";

        WebClient client;

        public ImportWiki()
        {
            client = new WebClient();

            if (!Directory.Exists(map_path))
            {
                Directory.CreateDirectory(map_path);
            }
        }

        private static readonly Regex FileNameRegex = new Regex("[^a-zA-Z0-9_.]", RegexOptions.Compiled);

        private static string GetName(string url)
        {
            return FileNameRegex.Replace(url, "_");
        }

        public HtmlPage GetPage(string url)
        {
            var name = GetName(url);
            var path = Path.Combine(map_path, name);

            string html;

            if (File.Exists(path))
            {
                html = File.ReadAllText(path);
            }
            else
            {
                html = client.DownloadString(url);
                File.WriteAllText(path, html);
            }

            return new HtmlPage(name, html);
        }

        public TokenPage Import(string page_url)
        {
            var htmlPage = GetPage(page_url);
            return htmlPage.ToTokenPage();
        }

        public List<TokenPage> ImportRecursive(string cathegory_url)
        {
            List<string> page_urls = Spider(cathegory_url);
            return page_urls.Select(Import).ToList();
        }

        private static List<string> Spider(string cathegory_url)
        {
            var client = new WebClient();
            string data = client.DownloadString(cathegory_url);

            List<string> urls = new List<string>();
            string item_open = "href=\"";
            string item_close = "\"";

            int current_index = 0;
            while (true)
            {
                int item_open_index = data.IndexOf(item_open, current_index);
                if (item_open_index == -1)
                {
                    break;
                }
                else
                {
                    item_open_index += item_open.Length;
                    int item_close_index = data.IndexOf(item_close, item_open_index);
                    if (item_close_index == -1)
                    {
                        break;
                    }
                    else
                    {
                        current_index = item_close_index;
                        string item = data.Substring(item_open_index, item_close_index - item_open_index);
                        if (6 < item.Length && item.Substring(0, 6).Equals("/wiki/") && !item.Contains(":") && !item.Contains("/wiki/List"))
                        {
                            urls.Add("https://en.wikipedia.org" + item);
                        }
                    }
                }
            }
            return urls;
        }



        private List<string> GetUrls(string page_url)
        {
            var page = GetPage(page_url);
            var html = page.Html;
            List<string> urls = new List<string>();
            string item_open = "<li>";
            string item_close = "</li>";

            int current_index = 0;
            while (true)
            {
                int item_open_index = html.IndexOf(item_open, current_index);
                if (item_open_index == -1)
                {
                    break;
                }
                else
                {
                    item_open_index += item_open.Length;
                    int item_close_index = html.IndexOf(item_close, item_open_index);
                    if (item_close_index == -1)
                    {
                        break;
                    }
                    else
                    {
                        current_index = item_close_index;
                        string item = html.Substring(item_open_index, item_close_index - item_open_index);

                        //TODO format further
                        string ref_open = "href=\"";
                        string ref_close = "\"";
                        int ref_open_index = item.IndexOf(ref_open);
                        if (ref_open_index != -1)
                        {
                            ref_open_index += ref_open.Length;
                            int ref_close_index = item.IndexOf(ref_close, ref_open_index);


                            string value = item.Substring(ref_open_index, ref_close_index - ref_open_index);
                            if (6 < value.Length && value.Substring(0, 6).Equals("/wiki/"))
                            {

                                urls.Add("https://en.wikipedia.org" + value);
                            }
                        }



                    }
                }
            }
            return urls;
        }
    }
}
