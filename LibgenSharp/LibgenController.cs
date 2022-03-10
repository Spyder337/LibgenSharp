using LibgenSharp.Processors;
using Microsoft.VisualBasic.FileIO;

namespace LibgenSharp;

public class LibgenController
{
    public void PrintSearch(string isbn)
    {
        var entries = Search(isbn);
        Console.WriteLine($"Found entries for {isbn}: ");
        foreach (var entry in entries)
        {
            Console.WriteLine($"\t{entry}");
        }
    }

    public bool TryDownloading(string isbn, string preferredExtension = "pdf")
    {
        var entries = Search(isbn);
        if (entries.Length == 0) return false;
        BookEntry entry = entries[0];
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Extension == preferredExtension)
            {
                entry = entries[i];
                break;
            }
        }

        var url = entry.Urls[0];
        DlProcessor dlProcessor = new LibgenLolProcessor();
        var path = BuildPath(entry.Title, entry.Extension);
        var fs = File.Create(path);
        fs.Dispose();
        dlProcessor.Process(out bool result, url, path);
        return result;
    }
    
    protected BookEntry[] Search(string isbn)
    {
        var proc = new SearchProcessor();
        proc.Process(out var entries, isbn);
        return entries;
    }

    private string BuildPath(string bookTitle, string ext)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "LibgenSharp", "Downloads", $"{bookTitle}.{ext}");
    }
}