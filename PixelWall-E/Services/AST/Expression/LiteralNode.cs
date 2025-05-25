public class LiteralNode : ExpressionNode
{
    public override ExpressionType type { get; set; }
    public override object value { get; set; } = null!;
    public LiteralNode(CodeLocation location, string value) : base(location){this.value = value;}
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}