using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using LibgenSharp.Processors;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LibgenSharp;

public class LibgenController
{
    public static readonly HashSet<char> InvalidFileSystemChars;
    public static readonly string RootPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibgenSharp");
    public static string DownloadsPath = Path.Combine(RootPath, "Downloads");
    public static string ResultsPath = Path.Combine(RootPath, "Results");
    private static readonly string _isbnRegex = @"(\b978(?:-?\d){10}\b)|(\b978(?:-?\d){9}(?:-?X|x))|(\b(?:-?\d){10})\b|(\b(?:-?\d){9}(?:-?X|x)\b)";
    public DownloadProcessor Processor = new DownloadCurlProcessor();

    public LibgenController()
    {
        if (InvalidFileSystemChars.Count == 0)
        {
            foreach (var c in Path.GetInvalidPathChars())
            {
                InvalidFileSystemChars.Add(c);
            }
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                InvalidFileSystemChars.Add(c);
            }
        }
        if (!Directory.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }

        if (!Directory.Exists(Path.Combine(RootPath, "Downloads")))
        {
            Directory.CreateDirectory(Path.Combine(RootPath, "Downloads"));
        }
    }
    
    public void PrintSearch(string isbn, bool logToFile = false)
    {
        var entries = Search(isbn, logToFile);
        Console.WriteLine($"Found entries for {isbn}: ");
        foreach (var entry in entries)
        {
            Console.WriteLine($"\t{entry}");
        }
    }

    public void PrintSearchFromFile(string path, bool logToFile = false)
    {
        var results = SearchUsingFile(path, logToFile);
        
        List<SearchResultInfo> info = new();
        foreach (var res in results)
        {
            var tempInfo = new SearchResultInfo(res.Key, res.Value);
            info.Add(tempInfo);
        }

        foreach (var res in info)
        {
            Console.WriteLine(res);
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
        DownloadProcessor downloadProcessor = new WebClientProcessors.LibgenLolWebClientProcessor();
        var path = BuildPath(entry.Title, entry.Extension);
        var fs = File.Create(path);
        fs.Dispose();
        downloadProcessor.Process(out bool result, url, path);
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
            Thread.Sleep(3000);
        }
        return count;
    }

    public Dictionary<string, BookEntry[]> SearchUsingFile(string path, bool logToFile = false)
    {
        Dictionary<string, BookEntry[]> res = new();
        if (!File.Exists(path))
            return res;
        using var sr = File.OpenText(path);
        var text = sr.ReadToEnd();
        MatchCollection matches = Regex.Matches(text, _isbnRegex);
        foreach (Match match in matches)
        {
            var isbn = match.Value;
            var query = Search(isbn);
            if (query.Length == 0) 
                continue;
            res.Add(isbn, query);
            Thread.Sleep(3000);
        }

        if (logToFile)
        {
            SerializeSearch(res);
        }
        return res;
    }
    
    protected BookEntry[] Search(string isbn, bool logToFile = false)
    {
        var proc = new SearchProcessor();
        proc.Process(out var entries, isbn);
        if (logToFile)
        {
            SerializeSearch(isbn, entries);
        }
        return entries;
    }

    private void SerializeSearch(string isbn, BookEntry[] results)
    {
        var path = Path.Combine(ResultsPath, $"{isbn}.json");
        SanitizePath(ref path);
        using var fs = File.Create(path);
        using var sw = new StreamWriter(fs);
        JsonObject? jObj = JsonSerializer.SerializeToNode(results)?.AsObject();
        if (jObj is not null)
        {
            sw.WriteLine(jObj);
        }
    }

    private void SerializeSearch(Dictionary<string, BookEntry[]> results)
    { 
        List<SearchResultInfo> info = new();
        foreach (var res in results)
        {
            var tempInfo = new SearchResultInfo(res.Key, res.Value);
            info.Add(tempInfo);
        }

        var infoJson = JsonSerializer.Serialize(info);
        
        var path = Path.Combine(ResultsPath, $"{DateTime.Now}.json");
        SanitizePath(ref path);
        using var fs = File.Create(path);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(infoJson);
    }

    public void SanitizePath(ref string newPath)
    {
        var sb = new StringBuilder();
        var chars = newPath.ToList();
        
        for (int i = 0; i < chars.Count; i++)
        {
            if (InvalidFileSystemChars.Contains(chars[i]))
            {
                chars.RemoveAt(i);
            }
        }

        sb.Append(chars);
        newPath = sb.ToString();
    }
    
    private string BuildPath(string bookTitle, string ext, bool organize = false)
    {
        string path = "";
        if (!organize)
        {
            bookTitle = bookTitle.Replace(":", "");
            path = Path.Combine(DownloadsPath, $"{bookTitle}.{ext}");
            SanitizePath(ref path);
        }
        return path;
    }
}