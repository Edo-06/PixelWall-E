public class BinaryOpNode : ExpressionNode
{
    public ExpressionNode left { get; }
    public TokenType op { get; }
    public ExpressionNode right { get; }
    public override ExpressionType type {get; set;} = ExpressionType.Unknow;
    public override object value {get; set;} = null!;

    public BinaryOpNode(CodeLocation location, ExpressionNode left, TokenType op, ExpressionNode right): base(location)
    {
        this.left = left;
        this.op = op;
        this.right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}