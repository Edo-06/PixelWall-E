public class VariableNode : ExpressionNode
{
    public string name { get; set; }
    public override object value { get; set; } = null!;
    public override ExpressionType type {get; set;}

    public VariableNode(CodeLocation location, string name) : base(location)
    {
        this.name = name;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}