using HtmlAgilityPack;
using LibgenSharp;
using LibgenSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LibgenBaseLib.Web
{
    public static class LibgenHandler
    {
        private static HtmlWeb _htmlWeb;
        private static readonly string _searchXPath = @"/html/body/table[3]";
        private static string Url { get; } = "https://libgen.is";
        public static HtmlWeb Client { get => _htmlWeb; }
        public static string UserAgent { get => "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11"; }

        static LibgenHandler()
        {
            if (_htmlWeb == null)
            {
                _htmlWeb = new HtmlWeb();
                _htmlWeb.UserAgent = UserAgent;
            }
        }

        /// <summary>
        /// Takes in a url from the search results and attempts to download the file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downloadHelper"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<bool> Download(string url, string path, IDownloader downloadHelper)
        {
            return await ProcessDownloadLink(url, path, downloadHelper);
        }

        public static async Task<BookEntry[]> SearchBookByIsbn(string isbn)
        {
            var url = BuildUrl(isbn);
            try
            {
                return await ProcessSearch(url);
            }
            catch (PageNotFoundException e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        internal static string GetLink(HtmlDocument doc, string xPath)
        {
            var node = doc.DocumentNode.SelectSingleNode(xPath);
            if (node == null)
                throw new LinkNotFoundException();
            var link = node.Attributes["href"].Value;
            return link;
        }

        internal static async Task<HtmlDocument?> GetDoc(string url)
        {
            if (_htmlWeb != null)
            {
                var doc = await _htmlWeb.LoadFromWebAsync(url);
                return doc;
            }
            return null;
        }

        private static async Task<bool> ProcessDownloadLink(string url, string path, IDownloader downloadHelper)
        {
            var doc = await GetDoc(url);
            if (doc == null)
            {
                throw new PageNotFoundException();
            }

            var link = GetLink(doc, downloadHelper.MainXPath);
            return await downloadHelper.TryDownload(link, path);
        }

        private static async Task<BookEntry[]> ProcessSearch(string url)
        {
            var doc = await _htmlWeb!.LoadFromWebAsync(url);
            var res = new List<BookEntry>();

            if (doc == null)
            {
                throw new PageNotFoundException();
            }

            var tableNode = doc.DocumentNode.SelectNodes(_searchXPath).First();
            var rows = tableNode.SelectNodes("tr");
            rows.Remove(0);

            foreach (var row in rows)
            {
                var entry = ConvertRowToBook(row);
                res.Add(entry);
            }

            return res.ToArray();
        }

        private static string BuildUrl(string isbn)
        {
            var builder = new UriBuilder(Url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["req"] = isbn;
            query["lg_topic"] = "libgen";
            query["open"] = "0";
            query["view"] = "simple";
            query["res"] = "100";
            query["phrase"] = "1";
            query["column"] = "identifier";
            builder.Query = query.ToString();
            var url = builder.ToString();
            return url;
        }

        private static BookEntry ConvertRowToBook(HtmlNode row)
        {
            var cols = row.SelectNodes("td");
            var urls = new string[3];
            for (int i = 0; i < 3; i++)
            {
                urls[i] = cols[i + 9].FirstChild.Attributes["href"].Value;
            }
            var book = new BookEntry()
            {
                Title = cols[2].InnerText,
                Authors = cols[1].InnerText,
                Extension = cols[8].InnerText,
                Urls = urls
            };
            return book;
        }
    }
}
