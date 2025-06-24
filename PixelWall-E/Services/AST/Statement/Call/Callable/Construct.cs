public static class Construct
{
    private static List<TokenPattern> TokenPatterns = new List<TokenPattern>();
    private static HashSet<TokenType> CallableCommandTokens = new HashSet<TokenType>();
    private static HashSet<TokenType> CallableExpressionTokens = new HashSet<TokenType>();
    private static Dictionary<TokenType,Callable> CallableInputs = new Dictionary<TokenType,Callable>();
    private static readonly HashSet<Callable> Callables = new HashSet<Callable>
    {
        new Callable(
            new TokenPattern(TokenType.Spawn, @"^[ \s+]*Spawn\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.MoveTo, @"^[ \t]*MoveTo\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.Color, @"^[ \t]*Color\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Color
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.Size, @"^[ \t]*Size\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.DrawLine, @"^[ \t]*DrawLine\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.DrawCircle, @"^[ \t]*DrawCircle\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.DrawRectangle, @"^[ \t]*DrawRectangle\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Void)
            ),
        new Callable(
            new TokenPattern(TokenType.Fill, @"^[ \t]*Fill\b"),
            new Expected(
                [],
                ExpressionType.Void)
            ),

        //Callable with return
        new Callable(
            new TokenPattern(TokenType.GetActualX, @"^[ \t]*GetActualX\b"),
            new Expected(
                [],
                ExpressionType.Number)
            ),
        new Callable(
            new TokenPattern(TokenType.GetActualY, @"^[ \t]*GetActualY\b"),
            new Expected(
                [],
                ExpressionType.Number)
            ),
        new Callable(
            new TokenPattern(TokenType.GetCanvasSize, @"^[ \t]*GetCanvasSize\b"),
            new Expected(
                [],
                ExpressionType.Number)
            ),
        new Callable(
            new TokenPattern(TokenType.GetColorCount, @"^[ \t]*GetColorCount\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Color,
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Number)
            ),
        new Callable(
            new TokenPattern(TokenType.IsBrushColor, @"^[ \t]*IsBrushColor\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Color
                },
                ExpressionType.Bool)
            ),
        new Callable(
            new TokenPattern(TokenType.IsBrushSize, @"^[ \t]*IsBrushSize\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Number
                },
                ExpressionType.Bool)
            ),
        new Callable(
            new TokenPattern(TokenType.IsCanvasColor, @"^[ \t]*IsCanvasColor\b"),
            new Expected(
                new List<ExpressionType>
                {
                    ExpressionType.Color,
                    ExpressionType.Number,
                    ExpressionType.Number
                },
                ExpressionType.Bool)
            ),
    };
    public static void Set()
    {
        CallableCommandTokens = new HashSet<TokenType>();
        CallableExpressionTokens = new HashSet<TokenType>();
        TokenPatterns = new List<TokenPattern>();
        CallableInputs = new Dictionary<TokenType, Callable>();
        foreach (var item in Callables)
        {
            if(item.expected.Output == ExpressionType.Void) //command
                CallableCommandTokens.Add(item.tokenPattern.Type);
            else
                CallableExpressionTokens.Add(item.tokenPattern.Type);

            TokenPatterns.Add(item.tokenPattern);
            CallableInputs.Add(item.tokenPattern.Type, item);
        }
    }
    public static bool IsCallableCommand(TokenType tokenType)
    {
        if(CallableCommandTokens.Contains(tokenType)) return true;
        return false;
    }
    public static bool IsCallableExpression(TokenType tokenType)
    {
        if(CallableExpressionTokens.Contains(tokenType)) return true;
        return false;
    }
    public static List<TokenPattern> GetTokenPatterns()
    {
        return TokenPatterns;
    }
    public static Callable GetElementByToken(TokenType tokenType)
    {
        return CallableInputs[tokenType];
    }
}