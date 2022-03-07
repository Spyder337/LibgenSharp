using LibgenSharp.Processors;

namespace LibgenSharp;

public class LibgenController
{
    public BookEntry[] Search(string isbn)
    {
        var proc = new SearchProcessor();
        proc.Process(out var entries, isbn);
        return entries;
    }

    public void PrintSearch(string isbn)
    {
        var entries = Search(isbn);
        Console.WriteLine("Found entries: ");
        foreach (var entry in entries)
        {
            Console.WriteLine($"\t{entry}");
        }
    }
}