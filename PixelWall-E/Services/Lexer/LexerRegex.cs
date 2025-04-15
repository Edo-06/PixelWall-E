using System.Collections.Generic;
public class LexerRegex
{
    public List<TokenPattern> tokenPatterns = new List<TokenPattern> 
    {
        // Command
        new TokenPattern(TokenType.Spawn, @"^Spawn\b"),
        new TokenPattern(TokenType.Color, @"^Color\b"),
        new TokenPattern(TokenType.Size, @"^Size\b"),
        new TokenPattern(TokenType.DrawLine, @"^DrawLine\b"),
        new TokenPattern(TokenType.DrawCircle, @"^DrawCircle\b"),
        new TokenPattern(TokenType.DrawRectangle, @"^DrawRectangle\b"),
        new TokenPattern(TokenType.Fill, @"^Fill\b"),

        // Function
        new TokenPattern(TokenType.GetActualX, @"^GetActualX\b"),
        new TokenPattern(TokenType.GetActualY, @"^GetActualY\b"),
        new TokenPattern(TokenType.GetCanvasSize, @"^GetCanvasSize\b"),
        new TokenPattern(TokenType.GetColorCount, @"^GetColorCount\b"),
        new TokenPattern(TokenType.IsBrushColor, @"^IsBrushColor\b"),
        new TokenPattern(TokenType.IsBrushSize, @"^IsBrushSize\b"),
        new TokenPattern(TokenType.IsCanvasColor, @"^IsCanvasColor\b"),

        // Operator
        new TokenPattern(TokenType.Power, @"^\*\*"),
        new TokenPattern(TokenType.Plus, @"^\+"),
        new TokenPattern(TokenType.Minus, @"^-"),
        new TokenPattern(TokenType.Multiply, @"^\*"),
        new TokenPattern(TokenType.Divide, @"^/"),
        new TokenPattern(TokenType.Modulo, @"^%"),


        // ComparisionOperator
        new TokenPattern(TokenType.Greater, @"^>"),
        new TokenPattern(TokenType.Less, @"^<"),
        new TokenPattern(TokenType.GreaterEqual, @"^>="),
        new TokenPattern(TokenType.LessEqual, @"^<="),
        new TokenPattern(TokenType.NotEqual, @"^!="),
        new TokenPattern(TokenType.Equal, @"^=="),

        // BooleanOperator
        new TokenPattern(TokenType.And, @"^&&"),
        new TokenPattern(TokenType.Or, @"^\|\|"),

        // Symbol
        new TokenPattern(TokenType.AssignArrow, @"^<-"),
        new TokenPattern(TokenType.LeftParen, @"^\("),
        new TokenPattern(TokenType.RightParen, @"^\)"),
        new TokenPattern(TokenType.Comma, @"^,"),
        new TokenPattern(TokenType.LeftBracket, @"^\["),
        new TokenPattern(TokenType.RightBracket, @"^\]"),

        // Literal
        new TokenPattern(TokenType.Number, @"^-?\d+"),
        new TokenPattern(TokenType.Identifier, @"^[a-zA-Z][a-zA-Z0-9-]*"),

        //Control
        new TokenPattern(TokenType.Label, @"^[a-zA-Z][a-zA-Z0-9-]*\r?\n"),
        new TokenPattern(TokenType.Goto, @"^GoTo\b"),
        new TokenPattern(TokenType.EndOfLine, @"^\r?\n"),

    };

}