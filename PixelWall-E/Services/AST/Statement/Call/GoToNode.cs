public class GoToNode: StatementNode, ICallableNode
{
    public TokenType tokenType { get ; set; }
    public List<ExpressionNode> parameters { get; set; } = [];
    public readonly int excpectedParametersCount = 1;
    public LabelNode label { get; set; } = null!;
    public GoToNode(CodeLocation location):base(location)
    {
        tokenType = TokenType.GoTo;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}