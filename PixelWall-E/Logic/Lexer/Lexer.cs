// Lexer.cs
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BlazorMonaco;

public class Lexer
{
    public string code { get; private set; }
    private int position;
    public int line;
    public int last;
    private LexerRegex lexerRegex = new LexerRegex();
    private List<Token> tokens = new List<Token>();

    public Lexer(string code)
    {
        this.code = code;
        position = 0;
        line = 1;
        last = -1;
    }

    public List<Token> GetTokens()
    {
        while (!IsAtEOF())
        {
            ScanToken();
        }

        return tokens;
    }

    private void ScanToken()
    {
        foreach (var pattern in lexerRegex.tokenPatterns)
        {
            var match = pattern.Regex.Match(code.Substring(position));
            
            if (match.Success)
            {
                string lexeme = match.Value;
                TokenType type = pattern.Type;

                tokens.Add(new Token(type, lexeme, location));
                position += lexeme.Length;
                UpdateLineAndColumn(lexeme);
                return;
            }
        }
        throw new LexerException($"Carácter inesperado: '{code[position]}'", location);
    }
    private void UpdateLineAndColumn(string lexeme)
    {
        foreach (char c in lexeme)
        {
            if (c == '\n')
            {
                line++;
                last = -1;
            }
            else
            {
                last++;
            }
        }
    }
    private bool IsAtEOF()
    {
        if(position >= code.Length) return true;
        return false;
    } 
    private CodeLocation location
    {
        get
        {
            return new CodeLocation
            {
                line = this.line,
                column = position - last
            };
        }
    }
}