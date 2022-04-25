using System.Net;
using HtmlAgilityPack;
using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public abstract class DlProcessor : Processor<bool>
{
    protected DlProcessor() : base(){}
    
    public DlProcessor(string linkXpath) : base(linkXpath)
    {
        
    }

    public override void Process(out bool result, params string[] args)
    {
        if (args.Length != 2) throw new InvalidNumOfArgsException(2, args.Length);
        var url = args[0];
        var path = args[1];
        ProcessLink(out result, url, path);
    }

    protected override void ProcessLink(out bool result, params string[] args)
    {
        var url = args[0];
        var path = args[1];
        
        var doc = GetDoc(url);
        if (doc == null)
        {
            throw new PageNotFoundException();
        }

        var link = GetLink(doc, _linkXpath);
        Console.WriteLine(link);
        result = TryDownloading(link, path);
        Console.WriteLine($"{result}");
    }

    protected virtual bool TryDownloading(string url, string path)
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

    protected string GetLink(HtmlDocument doc, string xPath)
    {
        var node = doc.DocumentNode.SelectSingleNode(xPath);
        if (node == null)
            throw new LinkNotFoundException();
        var link = node.Attributes["href"].Value;
        return link;
    }
}

public class ZLibraryProcessor : DlProcessor
{
    private string _subLinkXpath = @"/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div[1]/div[1]/div/a";
    public ZLibraryProcessor()
    {
        _linkXpath = @"/html/body/table/tbody/tr[2]/td/div/div/div/div[2]/div[2]/div/table/tbody/tr/td[2]/table/tbody/tr[1]/td/h3/a";
    }

    protected override bool TryDownloading(string url, string path)
    {
        var doc = GetDoc(url);
        if (doc == null)
        {
            throw new PageNotFoundException();
        }
        var subLink = GetLink(doc, _subLinkXpath);
        return base.TryDownloading(subLink, path);
    }
}

public class LibgenLolProcessor : DlProcessor
{
    public LibgenLolProcessor()
    {
        _linkXpath = @"//*[@id=""download""]/h2/a";
    }
}

public class LibgenRockProcessor : DlProcessor
{
    public LibgenRockProcessor()
    {
        _linkXpath = @"//*[@id=""main""]/tbody/tr[1]/td[2]/a";
    }
}