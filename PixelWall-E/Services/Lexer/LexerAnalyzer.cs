public class LexerAnalyzer
{
    public List<Token> GetTokens(string code)
    {
        List<Token> tokens = new List<Token>();
        Reader reader = new Reader(code);

        while (!reader.IsAtEnd())
        {
            reader.ScanToken(tokens);
        }

        return tokens;
    }

    private class Reader
    {
        private LexerRegex lexerRegex = new LexerRegex();
        private string code;
        private int position;
        private int line;
        private int last;

        public Reader(string code)
        {
            this.code = code;
            position = 0;
            line = 1;
            last = -1;
        }

        public bool IsAtEnd()
        {
            if(position >= code.Length) return true;
            return false;
        }
        public void ScanToken(List<Token> tokens)
        {
            foreach (var pattern in lexerRegex.tokenPatterns)
            {
                var match = pattern.Regex.Match(code.Substring(position));
                
                if (match.Success)
                {
                    string lexeme = match.Value;
                    TokenType type = pattern.Type;

                    tokens.Add(new Token(type, location, lexeme));
                    position += lexeme.Length;
                    UpdateLineAndColumn(lexeme);
                    return;
                }
            }
            throw new LexerException($"Carácter inesperado: '{code[position]}'", location);
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
    }
}