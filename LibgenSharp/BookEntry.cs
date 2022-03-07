using System.Text;

namespace LibgenSharp;

public struct BookEntry
{
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Extension { get; set; }
    public string[] Urls { get; set; }

    public BookEntry()
    {
        Title = "";
        Authors = "";
        Extension = "";
        Urls = new string[3];
    }
    
    public BookEntry(string title, string authors, string ext, string[] urls)
    {
        Title = title;
        Authors = authors;
        Extension = ext;
        Urls = urls;
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Title: {Title}");
        sb.AppendLine($"Authors: {Authors}");
        sb.AppendLine($"Extension: {Extension}");
        sb.AppendLine($"Urls:");
        foreach (var url in Urls)
        {
            sb.AppendLine($"\t{url}");
        }
        return sb.ToString();
    }
}