public class AssignmentNode : StatementNode
{
    public string name { get; set; }
    public ExpressionType type {get; set;}
    public ExpressionNode expression { get; set; } = null!;
    public AssignmentNode(CodeLocation location, string name) : base(location)
    {
        this.name = name;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}