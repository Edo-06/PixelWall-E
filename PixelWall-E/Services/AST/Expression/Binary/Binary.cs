public abstract class Binary: Expression
{
    public Expression? right;
    public Expression? left;
    public Binary(CodeLocation location) : base(location){}
}