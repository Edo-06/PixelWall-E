public class LabelNode : StatementNode
{
    public string name {get; set;}
    public int breakP {get; set;}
    public LabelNode(CodeLocation location, string name, int breakP) : base(location)
    {
        this.breakP = breakP;
        this.name = name;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}