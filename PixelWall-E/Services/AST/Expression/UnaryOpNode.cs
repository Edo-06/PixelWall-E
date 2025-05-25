public class UnaryOpNode : ExpressionNode
{
    public TokenType op { get; }
    public ExpressionNode operand { get; }
    public override ExpressionType type { get; set; } = ExpressionType.Unknow;
    public override object value { get; set; } = null!;

    public UnaryOpNode(CodeLocation location, TokenType op, ExpressionNode operand): base(location)
    {
        this.op = op;
        this.operand = operand;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}