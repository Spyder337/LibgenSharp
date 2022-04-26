// See https://aka.ms/new-console-template for more information

using System.Net;
using LibgenSharp;

LibgenController controller = new LibgenController();

if (args[0] == "-h")
{
    Console.WriteLine("-h\t:\tHelp\n\tDisplays a list of commands.");
    Console.WriteLine("-d\t:\tDownload\n\tDownloads a book when provided an isbn and an optional extension.");
    Console.WriteLine("-df\t:\tDownloadFile\n\tParses a text file containing isbns.");
    Console.WriteLine("-s\t:\tSearch\n\tDisplays search results for a book.");
    Console.WriteLine("-sf\t:\tSearchFile\n\tDisplays search results from a file of isbns.");
}
else if (args[0] == "-d")
{
    if (args.Length == 1) return;
    if (args.Length == 3)
    {
        controller.TryDownloading(args[1], args[2]);
    }
    else
    {
        controller.TryDownloading(args[1]);
    }
}
else if (args[0] == "-df")
{
    if (args.Length == 1) return;
    if (args.Length == 3)
    {
        controller.TryDownloadingFromFile(args[1], args[2]);
    }
    else
    {
        int successes = controller.TryDownloadingFromFile(args[1]);
        Console.WriteLine($"Successful Downloads: {successes}");
    }
}
else if (args[0] == "-s")
{
    switch (args.Length)
    {
        case 2:
            controller.PrintSearch(args[1]);
            break;
        case 3:
            if (bool.TryParse(args[2], out bool log))
            {
                controller.PrintSearch(args[1], log);
            }
            break;
    }
}
else if (args[0] == "-sf")
{
    switch (args.Length)
    {
        case 2:
            controller.PrintSearchFromFile(args[1]);
            break;
        case 3:
            if (bool.TryParse(args[2], out bool log))
            {
                controller.PrintSearchFromFile(args[1], log);
            }
            break;
    }
}
else
{
    Console.WriteLine("Use -h for a list of commands.");
}
