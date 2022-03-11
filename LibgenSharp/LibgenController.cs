using System.Text.RegularExpressions;
using LibgenSharp.Processors;
using Microsoft.VisualBasic.FileIO;

namespace LibgenSharp;

public class LibgenController
{
    public static readonly string RootPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibgenSharp");
    private static readonly string _isbnRegex = @"(\b978(?:-?\d){10}\b)|(\b978(?:-?\d){9}(?:-?X|x))|(\b(?:-?\d){10})\b|(\b(?:-?\d){9}(?:-?X|x)\b)";

    public LibgenController()
    {
        if (!Directory.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }

        if (!Directory.Exists(Path.Combine(RootPath, "Downloads")))
        {
            Directory.CreateDirectory(Path.Combine(RootPath, "Downloads"));
        }
    }
    
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
            if (entries[i].Extension != preferredExtension) 
                continue;
            entry = entries[i];
            break;
        }

        var url = entry.Urls[0];
        DlProcessor dlProcessor = new LibgenLolProcessor();
        var path = BuildPath(entry.Title, entry.Extension);
        var fs = File.Create(path);
        fs.Dispose();
        dlProcessor.Process(out bool result, url, path);
        return result;
    }

    public int TryDownloadingFromFile(string path, string preferredExtension = "pdf")
    {
        int count = 0;
        if (!File.Exists(path))
            return count;
        using var sr = File.OpenText(path);
        var text = sr.ReadToEnd();
        MatchCollection matches = Regex.Matches(text, _isbnRegex);
        foreach (Match match in matches)
        {
            var isbn = match.Value;
            if (!TryDownloading(isbn)) 
                continue;
            
            count++;
            Thread.Sleep(2000);
        }
        return count;
    }
    
    protected BookEntry[] Search(string isbn)
    {
        var proc = new SearchProcessor();
        proc.Process(out var entries, isbn);
        return entries;
    }

    private string BuildPath(string bookTitle, string ext)
    {
        return Path.Combine(RootPath, "Downloads", $"{bookTitle}.{ext}");
    }
}