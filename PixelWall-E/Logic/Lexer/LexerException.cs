public class LexerException : Exception
{
    public CodeLocation location { get; }

    public LexerException(string message, CodeLocation location) 
        : base($"{message} (Línea {location.line}, Columna {location.column})")
    {
        this.location = location;
    }
}