using System.Web;
using HtmlAgilityPack;
using LibgenSharp.Exceptions;

namespace LibgenSharp.Processors;

public class SearchProcessor : Processor<BookEntry[]>
{
    public static string Url { get; } = "https://libgen.is";
    
    public SearchProcessor() : base()
    {
        _linkXpath = @"/html/body/table[3]";
    }
    
    public SearchProcessor(string linkXpath) : base(linkXpath)
    {
    }

    public override void Process(out BookEntry[] result, params string[] args)
    {
        if (_webClient == null)
            throw new NullReferenceException("The web client was not initialized");
        var isbn = args[0];
        var url = BuildUrl(isbn);
        try
        {
            ProcessLink(out result, url);
        }
        catch (PageNotFoundException e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    protected override void ProcessLink(out BookEntry[] result, params string[] args)
    {
        
        var url = args[0];
        var doc = _htmlWeb!.Load(url);
        var res = new List<BookEntry>();
        
        if (doc == null)
        {
            throw new PageNotFoundException();
        }
        
        var tableNode = doc.DocumentNode.SelectNodes(_linkXpath).First();
        var rows = tableNode.SelectNodes("tr");
        rows.Remove(0);

        foreach (var row in rows)
        {
            var entry = ConvertRowToBook(row);
            res.Add(entry);
        }
        
        result = res.ToArray();
    }

    private string BuildUrl(string isbn)
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

    private BookEntry ConvertRowToBook(HtmlNode row)
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