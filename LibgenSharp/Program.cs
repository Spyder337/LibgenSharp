// See https://aka.ms/new-console-template for more information

using LibgenSharp;

Console.WriteLine("Hello, World!");

LibgenController controller = new LibgenController();
controller.TryDownloading(args[1]);