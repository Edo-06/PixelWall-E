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
    Number, Identifier,
    
    // Control
    Goto, Label, EndOfLine, Error,
    
    // Symbol
    AssignArrow, LeftParen, RightParen, Comma, LeftBracket, RightBracket   
}
public struct Location
{
    public int Line;
    public int Column;
}
#region ClassToken
public abstract class Token
{
    public string Lexeme {get; protected set;}
    public TokenType Type {get; protected set;}
    public Location Location {get; protected set;}

    public Token(TokenType type, string lexeme, Location location)
    {
        Lexeme = lexeme;
        Type = type;
        Location = location;
    }
    public override string ToString()
    {
        return $"[{Type}] '{Lexeme}' at {Location.Line}:{Location.Column}";
    }
    public virtual object GetValue() => null;
}
#endregion

#region InheritFromToken
public class LiteralToken: Token
{
    private readonly object value;

    public LiteralToken(TokenType type, string lexeme, Location location, object value): base(type, lexeme, location) 
    {
        this.value = value;
    }
    public override object GetValue() => value;
}

public class CommandToken: Token
{
    public CommandToken(TokenType type, string lexeme, Location location): base(type, lexeme, location) {}
}
public class FunctionToken: Token
{
    public FunctionToken(TokenType type, string lexeme, Location location): base(type, lexeme, location) {}
}
public class OperatorToken: Token
{
    public OperatorToken(TokenType type, string lexeme, Location location): base(type, lexeme, location) {}
}
public class ControlToken: Token
{
    public ControlToken(TokenType type, string lexeme, Location location): base(type, lexeme, location) {}
}
public class SymbolToken: Token
{
    public SymbolToken(TokenType type, string lexeme, Location location): base(type, lexeme, location) {}
}
public class ErrorToken: Token
{
    public string Message {get; private set;}
    public ErrorToken(TokenType type, string lexeme, Location location, string message): base(type, lexeme, location)
    {
        Message = message;
    }
    public override string ToString()
    {
        return $"[{Type}] '{Lexeme}' at {Location.Line}:{Location.Column} - {Message}";
    }
}
#endregion