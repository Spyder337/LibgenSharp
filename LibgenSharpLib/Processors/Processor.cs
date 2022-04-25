using System.Net;
using HtmlAgilityPack;

namespace LibgenSharp.Processors;

public abstract class Processor<T>
{
    protected static HttpClient? _client = null;
    protected static HtmlWeb? _htmlWeb = null;
    protected static WebClient? _webClient = null;
    protected string _linkXpath = "";
    private string _userAgent =
        "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

    protected Processor()
    {
        if (_client == null)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
        }

        if (_htmlWeb == null)
        {
            _htmlWeb = new HtmlWeb();
            _htmlWeb.UserAgent = _userAgent;
        }

        if (_webClient == null)
        {
            _webClient = new WebClient();
            _webClient.Headers.Add("user-agent", _userAgent);
        }
    }

    public Processor(string linkXpath) : this()
    {
        _linkXpath = linkXpath;
    }

    public abstract void Process(out T result, params string[] args);

    protected abstract void ProcessLink(out T result, params string[] args);

    protected HtmlDocument GetDoc(string url)
    {
        if (_htmlWeb != null)
        {
            var doc = _htmlWeb.Load(url);
            Thread.Sleep(1000);
            return doc;
        }
        return default;
    }
}