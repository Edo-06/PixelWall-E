using System.Collections.Generic;
public class LexerRegex
{
    public List<TokenPattern> tokenPatterns = new List<TokenPattern> 
    {
        // Command
        new TokenPattern(TokenType.Spawn, @"^\s*Spawn\b"),
        new TokenPattern(TokenType.Color, @"^\s*Color\b"),
        new TokenPattern(TokenType.Size, @"^\s*Size\b"),
        new TokenPattern(TokenType.DrawLine, @"^\s*DrawLine\b"),
        new TokenPattern(TokenType.DrawCircle, @"^\s*DrawCircle\b"),
        new TokenPattern(TokenType.DrawRectangle, @"^\s*DrawRectangle\b"),
        new TokenPattern(TokenType.Fill, @"^\s*Fill\b"),

        // Function
        new TokenPattern(TokenType.GetActualX, @"^\s*GetActualX\b"),
        new TokenPattern(TokenType.GetActualY, @"^\s*GetActualY\b"),
        new TokenPattern(TokenType.GetCanvasSize, @"^\s*GetCanvasSize\b"),
        new TokenPattern(TokenType.GetColorCount, @"^\s*GetColorCount\b"),
        new TokenPattern(TokenType.IsBrushColor, @"^\s*IsBrushColor\b"),
        new TokenPattern(TokenType.IsBrushSize, @"^\s*IsBrushSize\b"),
        new TokenPattern(TokenType.IsCanvasColor, @"^\s*IsCanvasColor\b"),

        // Operator
        new TokenPattern(TokenType.Power, @"^\s*\*\*"),
        new TokenPattern(TokenType.Plus, @"^\s*\+"),
        new TokenPattern(TokenType.Minus, @"^\s*-"),
        new TokenPattern(TokenType.Multiply, @"^\s*\*"),
        new TokenPattern(TokenType.Divide, @"^\s*/"),
        new TokenPattern(TokenType.Modulo, @"^\s*%"),


        // ComparisionOperator
        new TokenPattern(TokenType.Greater, @"^\s*>"),
        new TokenPattern(TokenType.Less, @"^\s*<"),
        new TokenPattern(TokenType.GreaterEqual, @"^\s*>="),
        new TokenPattern(TokenType.LessEqual, @"^\s*<="),
        new TokenPattern(TokenType.NotEqual, @"^\s*!="),
        new TokenPattern(TokenType.Equal, @"^\s*=="),

        // BooleanOperator
        new TokenPattern(TokenType.And, @"^\s*&&"),
        new TokenPattern(TokenType.Or, @"^\s*\|\|"),

        // Symbol
        new TokenPattern(TokenType.AssignArrow, @"^\s*<-"),
        new TokenPattern(TokenType.LeftParen, @"^\s*\("),
        new TokenPattern(TokenType.RightParen, @"^\s*\)"),
        new TokenPattern(TokenType.Comma, @"^\s*,"),
        new TokenPattern(TokenType.LeftBracket, @"^\s*\["),
        new TokenPattern(TokenType.RightBracket, @"^\s*\]"),

        // Literal
        new TokenPattern(TokenType.Number, @"^\s*-?\d+"),
        new TokenPattern(TokenType.Identifier, @"^\s*[a-zA-Z][a-zA-Z0-9-]*"),
        new TokenPattern(TokenType.ColorString, @"^\s*[""](Red|Blue|Green|Yellow|Orange|Purple|Black|White)[""]"),

        //Control
        new TokenPattern(TokenType.Label, @"^\s*[a-zA-Z][a-zA-Z0-9-]*\r?\n"),
        new TokenPattern(TokenType.Goto, @"^\s*GoTo\b"),
        new TokenPattern(TokenType.EndOfLine, @"^\s*\r?\n"),
    };

}