
public class CommandNode : StatementNode, ICallableNode
{
    public TokenType tokenType { get; set; }
    public List<ExpressionNode> parameters {get; set;} = [];
    public CommandNode(CodeLocation location, TokenType tokenType) : base(location)
    {
        this.tokenType = tokenType;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}