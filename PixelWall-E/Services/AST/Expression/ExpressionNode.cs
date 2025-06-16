public abstract class ExpressionNode : ASTNode 
{
    public abstract ExpressionType type { get; set; }
    public abstract object value { get; set; }
    public ExpressionNode(CodeLocation location) : base(location) {}
}
public enum ExpressionType
{
    Unknow,
    Color,
    Number,
    Bool,
    Void,
    ErrorType
}
