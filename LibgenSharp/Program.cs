// See https://aka.ms/new-console-template for more information

using LibgenSharp;

LibgenController controller = new LibgenController();

if (args[0] == "-h")
{
    Console.WriteLine("-h\t:\tHelp\n\tDisplays a list of commands.");
    Console.WriteLine("-d\t:\tDownload\n\tDownloads a book when provided an isbn and an optional extension.");
    Console.WriteLine("-df\t:\tDownloadFile\n\tParses a text file containing isbns.");
    Console.WriteLine("-s\t:\tHelp\n\tDisplays a list of commands.");
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
        controller.TryDownloadingFromFile(args[1]);
    }
}
else if (args[0] == "-s")
{
    if (args.Length == 1) return;
    controller.PrintSearch(args[1]);
}
else if (args[0] == "-sf")
{
    if (args.Length == 0) return;
    controller.SearchUsingFile(args[1]);
}
else
{
    Console.WriteLine("Use -h for a list of commands.");
}
