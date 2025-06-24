public class LexerException : Exception
{
    public LexerErrorCode code { get; private set; }
    public string message { get; private set; } 
    public CodeLocation location { get; private set; } 

    public LexerException(CodeLocation location, LexerErrorCode code, string message)
        : base($"{message}: (line {location.line}, column {location.column})")
    {
        this.location = location;
        this.code = code;
        this.message = message;
    }
    public override string ToString()
    {
        return $"Lexer Exception: {code} {message} at line {location.line}, column {location.column}";
    }
}

public enum LexerErrorCode
{
    UnexpectedCharacter,
}