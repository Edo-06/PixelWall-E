using System.Text.RegularExpressions;
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