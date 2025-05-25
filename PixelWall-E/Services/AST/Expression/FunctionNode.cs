
public class FunctionNode : ExpressionNode, ICallableNode
{
    public FunctionNode(CodeLocation location) : base(location){}

    public override ExpressionType type { get; set; }
    public override object value { get; set; } = null!;
    public List<ExpressionNode> parameters { get ; set ; } = [];
    public TokenType tokenType { get ; set ; }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}