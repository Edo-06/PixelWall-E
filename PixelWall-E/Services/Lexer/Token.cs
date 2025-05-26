public enum TokenType
{
    // Command
    Spawn, Color, Size, DrawLine, DrawCircle, DrawRectangle, Fill,
    
    // Functions
    GetActualX, GetActualY, GetCanvasSize, GetColorCount,
    IsBrushColor, IsBrushSize, IsCanvasColor,
    
    // AritmeticOperator
    Plus, Minus, Multiply, Divide, Power, Modulo,

    //BooleanOperator
    And, Or, Not,

    // ComparisionOperator
    Equal, NotEqual, Greater, Less, GreaterEqual, LessEqual,
    
    // Literal
    Number, ColorString, Bool,
    
    // Control
    GoTo, Identifier, EndOfLine,
    
    // Symbol
    AssignArrow, LeftParen, RightParen, Comma, LeftBracket, RightBracket   
}
public struct CodeLocation
{
    public int line;
    public int column;
}
#region ClassToken
public class Token
{
    public string lexeme {get; private set;}
    public TokenType type {get; private set;}
    public CodeLocation location {get; private set;}

    public Token(TokenType type, CodeLocation location, string lexeme)
    {
        this.lexeme = lexeme;
        this.type = type;
        this.location = location;
    }
    public bool IsCallable() => callables.Contains(type);
    public bool IsController() => controllers.Contains(type);
    public bool IsCallableExpression() => callablesWhitReturn.Contains(type);
    public override string ToString()
    {
        return $"[{type}] '{lexeme}' at {location.line}:{location.column}";
    }
    private readonly HashSet<TokenType> callables = new HashSet<TokenType>
    {
        TokenType.Spawn, TokenType.Color, TokenType.Size, TokenType.DrawLine,
        TokenType.DrawCircle, TokenType.DrawRectangle, TokenType.Fill,
    };
    private readonly HashSet<TokenType> callablesWhitReturn = new HashSet<TokenType>
    {
        TokenType.GetActualX, TokenType.GetActualY, TokenType.GetCanvasSize, TokenType.GetColorCount,
        TokenType.IsBrushColor, TokenType.IsBrushSize, TokenType.IsCanvasColor
    };
    private readonly HashSet<TokenType> controllers = new HashSet<TokenType>
    {
        TokenType.GoTo, TokenType.Identifier, TokenType.EndOfLine,
    };

    public readonly IReadOnlyDictionary<TokenType, int> expectedParameterCounts = new Dictionary<TokenType, int>
    {
        // Command
        { TokenType.Spawn, 2 },
        { TokenType.Color, 1 },
        { TokenType.Size, 1 },
        { TokenType.DrawLine, 3 },
        { TokenType.DrawCircle, 3 },
        { TokenType.DrawRectangle, 5 },
        { TokenType.Fill, 0 },

        // Function
        { TokenType.GetActualX, 0 },
        { TokenType.GetActualY, 0 },
        { TokenType.GetCanvasSize, 0 },
        { TokenType.GetColorCount, 0 },
        { TokenType.IsBrushColor, 1 },
        { TokenType.IsBrushSize, 1 },
        { TokenType.IsCanvasColor, 1 },

        {TokenType.GoTo, 1}
    };
}
#endregion
