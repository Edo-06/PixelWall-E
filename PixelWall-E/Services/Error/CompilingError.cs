public class CompilingError: Exception
{
    public ErrorCode code { get; private set; }

    public string message { get; private set; }

    public CodeLocation location {get; private set;}

    public CompilingError(CodeLocation location, ErrorCode code, string message)
    :base($"{message}: (line {location.line}, column {location.column}) ")
    {
        this.code = code;
        this.message = message;
        this.location = location;
    }
    public override string ToString()
    {
        return $"Compiling Error: {message} at line {location.line}, column {location.column}";
    }
}

    public enum ErrorCode
    {
        Expected,
        Invalid,
        Unexpected,
        Unknown,
    }