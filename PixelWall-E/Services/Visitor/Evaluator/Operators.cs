public static class Operators
{
    public static Dictionary<TokenType, string> OperatorSymbols = new Dictionary<TokenType, string>
    {
        { TokenType.Plus, "+" },
        { TokenType.Minus, "-" },
        { TokenType.Multiply, "*" },
        { TokenType.Divide, "/" },
        { TokenType.Power, "^" },
        { TokenType.Modulo, "%" },
        { TokenType.Equal, "==" },
        { TokenType.NotEqual, "!=" },
        { TokenType.Less, "<" },
        { TokenType.Greater, ">" },
        { TokenType.LessEqual, "<=" },
        { TokenType.GreaterEqual, ">=" },
        { TokenType.And, "&&" },
        { TokenType.Or, "||" }
    };
}