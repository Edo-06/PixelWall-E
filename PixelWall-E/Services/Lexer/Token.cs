public enum TokenType
{
    // Command
    Spawn, Color, Size, DrawLine, DrawCircle, DrawRectangle, Fill, MoveTo,
    
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
    GoTo, Identifier, EndOfLine, EndOfFile, 
    
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
    public bool IsController() => controllers.Contains(type);
    public override string ToString()
    {
        return $"[{type}] '{lexeme}' at {location.line}:{location.column}";
    }
    private readonly HashSet<TokenType> controllers = new HashSet<TokenType>
    {
        TokenType.GoTo, TokenType.Identifier, TokenType.EndOfLine,
    };
}
#endregion
