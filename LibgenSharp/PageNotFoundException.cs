namespace LibgenSharp;

public class PageNotFoundException : Exception
{
    public override string Message { get; } = "The webpage was not found at the requested url.";
}