using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LexerRegex
{
    public List<TokenPattern> Patterns = new List<TokenPattern>
    {
        // Command
        new TokenPattern(TokenType.Spawn, @"^Spawn\b", typeof(CommandToken)),
        new TokenPattern(TokenType.Color, @"^Color\b", typeof(CommandToken)),
        new TokenPattern(TokenType.Size, @"^Size\b", typeof(CommandToken)),
        new TokenPattern(TokenType.DrawLine, @"^DrawLine\b", typeof(CommandToken)),
        new TokenPattern(TokenType.DrawCircle, @"^DrawCircle\b", typeof(CommandToken)),
        new TokenPattern(TokenType.DrawRectangle, @"^DrawRectangle\b", typeof(CommandToken)),
        new TokenPattern(TokenType.Fill, @"^Fill\b", typeof(CommandToken)),

        // Function
        new TokenPattern(TokenType.GetActualX, @"^GetActualX\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.GetActualY, @"^GetActualY\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.GetCanvasSize, @"^GetCanvasSize\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.GetColorCount, @"^GetColorCount\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.IsBrushColor, @"^IsBrushColor\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.IsBrushSize, @"^IsBrushSize\b", typeof(FunctionToken)),
        new TokenPattern(TokenType.IsCanvasColor, @"^IsCanvasColor\b", typeof(FunctionToken)),

        // Operator
        new TokenPattern(TokenType.Power, @"^\*\*", typeof(OperatorToken)),
        new TokenPattern(TokenType.AssignArrow, @"^<-", typeof(OperatorToken)),
        new TokenPattern(TokenType.GreaterEqual, @"^>=", typeof(OperatorToken)),
        new TokenPattern(TokenType.LessEqual, @"^<=", typeof(OperatorToken)),
        new TokenPattern(TokenType.NotEqual, @"^!=", typeof(OperatorToken)),
        new TokenPattern(TokenType.And, @"^&&", typeof(OperatorToken)),
        new TokenPattern(TokenType.Or, @"^\|\|", typeof(OperatorToken)),
        new TokenPattern(TokenType.Equal, @"^==", typeof(OperatorToken)),
        new TokenPattern(TokenType.Plus, @"^\+", typeof(OperatorToken)),
        new TokenPattern(TokenType.Minus, @"^-", typeof(OperatorToken)),
        new TokenPattern(TokenType.Multiply, @"^\*", typeof(OperatorToken)),
        new TokenPattern(TokenType.Divide, @"^/", typeof(OperatorToken)),
        new TokenPattern(TokenType.Modulo, @"^%", typeof(OperatorToken)),
        new TokenPattern(TokenType.Greater, @"^>", typeof(OperatorToken)),
        new TokenPattern(TokenType.Less, @"^<", typeof(OperatorToken)),
        new TokenPattern(TokenType.LeftParen, @"^\(", typeof(OperatorToken)),
        new TokenPattern(TokenType.RightParen, @"^\)", typeof(OperatorToken)),
        new TokenPattern(TokenType.Comma, @"^,", typeof(OperatorToken)),
        new TokenPattern(TokenType.LeftBracket, @"^\[", typeof(OperatorToken)),
        new TokenPattern(TokenType.RightBracket, @"^\]", typeof(OperatorToken)),

        // Literal
        new TokenPattern(TokenType.Number, @"^-?\d+", typeof(LiteralToken)),
        new TokenPattern(TokenType.Identifier, @"^[a-zA-Z_][a-zA-Z_0-9]*", typeof(LiteralToken)),

        //Control
        new TokenPattern(TokenType.Label, @"^[a-zA-Z_][a-zA-Z_0-9]*:", typeof(ControlToken)),
        new TokenPattern(TokenType.Goto, @"^GoTo\b", typeof(ControlToken)),
        new TokenPattern(TokenType.EndOfLine, @"^\r?\n", typeof(ControlToken)),

    };

    public class TokenPattern
    {
        public TokenType Type { get; private set; }
        public Regex Regex { get; private set; }
        public System.Type TokenClass { get; private set; }

        public TokenPattern(TokenType type, string pattern, System.Type tokenClass)
        {
            Type = type;
            Regex = new Regex(pattern, RegexOptions.Compiled);
            TokenClass = tokenClass;
        }
    }
}