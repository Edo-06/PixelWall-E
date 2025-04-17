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
    And, Or, 

    // ComparisionOperator
    Equal, NotEqual, Greater, Less, GreaterEqual, LessEqual,
    
    // Literal
    Number, Identifier, ColorString,
    
    // Control
    Goto, Label, EndOfLine, Whitespace,
    
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

    public Token(TokenType type, string lexeme, CodeLocation location)
    {
        this.lexeme = lexeme;
        this.type = type;
        this.location = location;
    }
    public override string ToString()
    {
        return $"[{type}] '{lexeme}' at {location.line}:{location.column}";
    }
}
#endregion
