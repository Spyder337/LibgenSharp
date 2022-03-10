namespace LibgenSharp;

public static class Program
{
    public static readonly string RootPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibgenSharp");
    public static void Main(params string[] args)
    {
        LibgenController controller = new LibgenController();
        controller.PrintSearch("978-0-9858117-5-4");
        Console.WriteLine();
        bool res = controller.TryDownloading("978-0-9858117-5-4");
        Console.WriteLine($"Completed successfully: {res}");
    }
}

