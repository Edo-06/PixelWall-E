public abstract class Binary: Expression
{
    public Expression? Right;
    public Expression? Left;
    public Binary(CodeLocation location) : base(location){}
}