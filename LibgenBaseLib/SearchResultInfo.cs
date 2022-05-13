using System.Text;

namespace LibgenBaseLib;

[Serializable]
public struct SearchResultInfo
{
    public string Title { get; set; }
    public string Isbn { get; set; }
    public Dictionary<string, int> FileTypeCount { get; set; }
    public Dictionary<string, List<string>> FileTypeUrls { get; set; }

    public SearchResultInfo(string isbn, BookEntry[] entries)
    {
        Isbn = isbn;
        Title = entries[0].Title;
        FileTypeCount = new Dictionary<string, int>();
        FileTypeUrls = new Dictionary<string, List<string>>();
        foreach (var entry in entries)
        {
            if (!FileTypeCount.ContainsKey(entry.Extension))
            {
                FileTypeCount.Add(entry.Extension, 0);
            }
            FileTypeCount[entry.Extension]++;

            if (!FileTypeUrls.ContainsKey(entry.Extension))
            {
                FileTypeUrls.Add(entry.Extension, new());
            }

            foreach (var url in entry.Urls)
            {
                FileTypeUrls[entry.Extension].Add(url);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Title: {Title}");
        sb.AppendLine($"Isbn: {Isbn}");
        sb.AppendLine($"Urls: ");
        foreach (var pair in FileTypeUrls)
        {
            sb.AppendLine($"\tType: {pair.Key}");
            foreach (var val in pair.Value)
            {
                sb.AppendLine($"\t\t{val}");
            }
        }
        return sb.ToString();
    }
}