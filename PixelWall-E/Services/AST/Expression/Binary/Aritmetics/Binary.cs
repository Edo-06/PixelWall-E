public abstract class Binary: Expression
{
    public Expression? right;
    public Expression? left;
    public Binary(CodeLocation location, Expression left, Expression right) : base(location)
    {
        this.right = right;
        this.left = left;
    }
}