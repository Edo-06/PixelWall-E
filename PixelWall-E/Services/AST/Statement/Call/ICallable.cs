public interface ICallableNode
{
    TokenType tokenType { get; set; }
    List<ExpressionNode> parameters { get; set; }
}