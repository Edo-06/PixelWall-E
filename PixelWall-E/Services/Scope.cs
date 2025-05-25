public static class Scope
{
    public static Dictionary<string, ExpressionNode> variables = new Dictionary<string, ExpressionNode>();
    public static Dictionary<string, int> labels = new Dictionary<string, int>();
}