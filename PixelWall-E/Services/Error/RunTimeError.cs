public class RuntimeError : Exception
{
    public RuntimeErrorCode code { get; private set; }
    public CodeLocation location { get; private set; }
    public string message { get; private set; }

    public RuntimeError(CodeLocation location, RuntimeErrorCode code, string message)
        : base($"{message}: (line {location.line}, column {location.column}) ")
    {
        this.location = location;
        this.code = code;
        this.message = message;
    }
    public override string ToString()
    {
        return $"Runtime Error: {message} at line {location.line}, column {location.column}";
    }
}

public enum RuntimeErrorCode
{
    DivisionByZero,
    OutOfBounds,
    TypeMismatch,
    InvalidOperation,
    CommandExecutionError,
    InvalidDirection,
    InfiniteLoopDetected,
    UnhandledInternalError
}