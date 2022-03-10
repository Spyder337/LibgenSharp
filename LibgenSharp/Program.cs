namespace LibgenSharp;

public static class Program
{
    private static LibgenController _controller = new LibgenController();
    public static readonly string RootPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibgenSharp");
    public static void Main(params string[] args)
    {
        _controller.PrintSearch("978-0-9858117-5-4");
        Console.WriteLine();
        bool res = _controller.TryDownloading("978-0-9858117-5-4");
        Console.WriteLine($"Completed successfully: {res}");
    }
}

