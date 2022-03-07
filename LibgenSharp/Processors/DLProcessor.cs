using HtmlAgilityPack;

namespace LibgenSharp.Processors;

public abstract class DlProcessor : Processor<bool>
{
    public DlProcessor(string xpath) : base(xpath)
    {
        
    }

    protected string GetLink(HtmlDocument doc)
    {
        return default;
    }
}