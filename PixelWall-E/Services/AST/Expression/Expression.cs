public abstract class Expression : Node
{
    public abstract ExpressionType type { get; set; }

    public abstract object? value { get; set; }

    public Expression(CodeLocation location) : base (location) {}
}
public enum ExpressionType
{
    Anytype,
    Color,
    Number,
    Bool,
    Label,
    ErrorType
}