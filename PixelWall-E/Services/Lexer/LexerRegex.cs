using System.Text.RegularExpressions;
public class LexerRegex
{
    public List<TokenPattern> tokenPatterns = new List<TokenPattern> 
    {
        //Control
        new TokenPattern(TokenType.GoTo, @"^[ \t]*GoTo\b"),
        new TokenPattern(TokenType.EndOfLine, @"^[ \t]*(\r\n|\n)"),

        // Operator
        new TokenPattern(TokenType.Power, @"^[ \t]*\*\*"),
        new TokenPattern(TokenType.Plus, @"^[ \t]*\+"),
        new TokenPattern(TokenType.Minus, @"^[ \t]*-"),
        new TokenPattern(TokenType.Multiply, @"^[ \t]*\*"),
        new TokenPattern(TokenType.Divide, @"^[ \t]*/"),
        new TokenPattern(TokenType.Modulo, @"^[ \t]*%"),

        // Symbol
        new TokenPattern(TokenType.AssignArrow, @"^[ \t]*<-"),
        new TokenPattern(TokenType.LeftParen, @"^[ \t]*\("),
        new TokenPattern(TokenType.RightParen, @"^[ \t]*\)"),
        new TokenPattern(TokenType.Comma, @"^[ \t]*,"),
        new TokenPattern(TokenType.LeftBracket, @"^[ \t]*\["),
        new TokenPattern(TokenType.RightBracket, @"^[ \t]*\]"),

        // ComparisionOperator
        new TokenPattern(TokenType.GreaterEqual, @"^[ \t]*>="),
        new TokenPattern(TokenType.LessEqual, @"^[ \t]*<="),
        new TokenPattern(TokenType.Greater, @"^[ \t]*>"),
        new TokenPattern(TokenType.Less, @"^[ \t]*<"),
        new TokenPattern(TokenType.NotEqual, @"^[ \t]*!="),
        new TokenPattern(TokenType.Equal, @"^[ \t]*=="),

        // BooleanOperator
        new TokenPattern(TokenType.And, @"^[ \t]*&&"),
        new TokenPattern(TokenType.Or, @"^[ \t]*\|\|"),

        // Literal
        new TokenPattern(TokenType.Number, @"^[ \t]*-?\d+"),
        new TokenPattern(TokenType.Bool, @"^[ \t]*true\b"),
        new TokenPattern(TokenType.Bool, @"^[ \t]*false\b"),
        new TokenPattern(TokenType.Identifier, @"^[ \t]*[a-zA-Z][a-zA-Z0-9_]*"),
        new TokenPattern(TokenType.ColorString, @"^[ \t]*""([#a-zA-Z0-9]*)"""),
        
        new TokenPattern(TokenType.EndOfFile, @"^\s+$"),
    };
    public List<TokenPattern> GetTokenPatterns()
    {
        List<TokenPattern> tokens = Construct.GetTokenPatterns();
        tokens.AddRange(tokenPatterns);
        return tokens;
    }
}
public class TokenPattern
{
    public TokenType Type { get; private set; }
    public Regex Regex { get; private set; }

    public TokenPattern(TokenType type, string pattern)
    {
        Type = type;
        Regex = new Regex(pattern, RegexOptions.Compiled);
    }
}