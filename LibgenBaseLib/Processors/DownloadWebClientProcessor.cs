using System.Net;
using HtmlAgilityPack;
using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public abstract class DownloadWebClientProcessor : DownloadProcessor
{
    protected DownloadWebClientProcessor() : base(){}
    
    public DownloadWebClientProcessor(string linkXpath) : base(linkXpath)
    {
        
    }

    public override bool TryDownload(string url, string path)
    {
        var uri = new Uri(url, UriKind.Absolute);
        Console.WriteLine(uri);
        try
        {
            var data = _webClient!.DownloadData(uri);
            using var fs = File.Create(path);
            fs.Write(data);
            fs.Flush();
            return true;
        }
        catch(WebException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}