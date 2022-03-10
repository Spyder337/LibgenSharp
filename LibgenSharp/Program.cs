namespace LibgenSharp;

public static class Program
{
    private static LibgenController _controller = new LibgenController();
    public static readonly string RootPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LibgenSharp");
    public static void Main(params string[] args)
    {
        if (args.Length is 2)
        {
            if (args[0] == "-s")
            {

                var res = _controller.TryDownloading(args[1]);
                Console.WriteLine($"Completed download: {res}");
            }
            else if (args[0] == "-fs")
            {

                var res = _controller.TryDownloadingFromFile(args[1]);
                Console.WriteLine($"Completed downloads: {res}");
            }
            else
            {
                Console.WriteLine("Invalid arguments passed.");
            }
        }
        else if (args.Length is 4)
        {
            if (args[0] == "-s")
            {
                if (args[2] == "-ft")
                {
                    var res = _controller.TryDownloading(args[1], args[3]);
                    Console.WriteLine($"Completed download: {res}");
                }
                else
                {
                    Console.WriteLine("Invalid arguments passed.");
                }
            }
            else if (args[0] == "-fs")
            {
                if (args[2] == "-ft")
                {
                    var res = _controller.TryDownloadingFromFile(args[1], args[3]);
                    Console.WriteLine($"Completed downloads: {res}");
                }
                else
                {
                    Console.WriteLine("Invalid arguments passed.");
                }
            }
            else
            {
                Console.WriteLine("Invalid arguments passed.");
            }
        }
        else
        {
            Console.WriteLine("Invalid arguments passed.");
        }
        /*
        _controller.PrintSearch("978-0-9858117-5-4");
        Console.WriteLine();
        bool res = _controller.TryDownloading("978-0-9858117-5-4");
        Console.WriteLine($"Completed successfully: {res}");
        */
    }
}

