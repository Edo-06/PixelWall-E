public class ProgramNode : StatementNode
{
    public List<ASTNode> statements { get; set;} = [];
    public ProgramNode(CodeLocation location):base(location){}
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}