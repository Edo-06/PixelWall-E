public class LabelNode : StatementNode
{
    public string name {get; set;}
    public LabelNode(CodeLocation location, string name) : base(location)
    {
        this.name = name;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}