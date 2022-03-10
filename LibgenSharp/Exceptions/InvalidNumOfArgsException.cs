namespace LibgenSharp.Exceptions;

public class InvalidNumOfArgsException : Exception
{
    public override string Message { get; }

    public InvalidNumOfArgsException(int required, int provided)
    {
        Message = $"Argument only accepts {required} args. {provided} were input.";
        throw new NotImplementedException();
    }
}