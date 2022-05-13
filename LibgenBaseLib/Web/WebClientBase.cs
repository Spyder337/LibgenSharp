using HtmlAgilityPack;
using LibgenSharp.Exceptions;
using System.Net;
using System.Text.Json.Serialization;

namespace LibgenBaseLib.Web
{
    public abstract class WebClientBase : IDownloader
    {
        protected static HtmlWeb? _htmlWeb = null;
        protected static HttpClient? _client = null;
        public abstract string[] XPaths { get; }

        public WebClientBase()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.DefaultRequestHeaders.UserAgent.ParseAdd(LibgenHandler.UserAgent);
            }
        }

        public virtual async Task<bool> TryDownload(string url, string destPath)
        {
            var uri = new Uri(url, UriKind.Absolute);
            Console.WriteLine(uri);
            try
            {
                var reqData = await _client!.GetByteArrayAsync(uri);
                if (reqData == null)
                    return false;
                using var fs = File.Create(destPath);
                await fs.WriteAsync(reqData);
                fs.Flush();
                return true;
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }

    public class WebClientLol : WebClientBase
    {
        [JsonPropertyName("LastXPath")]
        internal static int _currXPath;
        public override string[] XPaths { get; } = new string[] {
            @"//*[@id=""download""]/h2/a",
            @"//*[@id=""download""]/ul/li[1]/a",
            @"//*[@id=""download""]/ul/li[2]/a",
            @"//*[@id=""download""]/ul/li[3]/a",
            @"//*[@id=""download""]/ul/li[4]/a"
        };

        public override async Task<bool> TryDownload(string url, string destPath)
        {
            var doc = await LibgenHandler.GetDoc(url);
            if (doc == null)
            {
                throw new PageNotFoundException();
            }
            var xPath = SelectXPath();
            string downloadUrl = LibgenHandler.GetLink(doc, xPath);
            return await base.TryDownload(downloadUrl, destPath);
        }

        private string SelectXPath()
        {
            _currXPath++;
            if (_currXPath > XPaths.Length)
                _currXPath = 0;
            return XPaths[_currXPath];
        }
    }
}
