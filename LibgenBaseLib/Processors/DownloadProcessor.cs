using HtmlAgilityPack;
using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public abstract class DownloadProcessor : Processor<bool>, IDownloadProcessor<bool>
{
    protected DownloadProcessor() : base(){}
    
    public DownloadProcessor(string linkXpath) : base(linkXpath)
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
        result = TryDownload(link, path);
    }
    
    protected string GetLink(HtmlDocument doc, string xPath)
    {
        var node = doc.DocumentNode.SelectSingleNode(xPath);
        if (node == null)
            throw new LinkNotFoundException();
        var link = node.Attributes["href"].Value;
        return link;
    }

    public abstract bool TryDownload(string link, string path);
}