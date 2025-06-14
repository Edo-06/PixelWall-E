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
        if(tokens.Count > 0 && tokens[^1].type != TokenType.EndOfFile)
        {
            tokens.Add(new Token(TokenType.EndOfFile, reader.location, ""));
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
            last = 0;
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
                    
                    CodeLocation l = location;
                    Console.WriteLine($"Token encontrado: {type} con lexema '{lexeme}' en línea {location.line}, columna {location.column}");

                    position += lexeme.Length;
                    UpdateLineAndColumn(lexeme);
                    lexeme = lexeme.Trim();
                    tokens.Add(new Token(type, l, lexeme));
                    return;
                    throw new LexerException($"Carácter inesperado: '{code[position]}' ", location);
                }
            }
        }

        public CodeLocation location
        {
            get
            {
                return new CodeLocation
                {
                    line = line,
                    column = last
                };
            }
        }
        private void UpdateLineAndColumn(string lexeme)
        {
            foreach (char c in lexeme)
            {
                //Console.WriteLine($"Procesando carácter: '{c}' en línea {line}, posición {last + 1}");
                if (c == '\n')
                {
                    line++;
                    last = 0;
                    Console.WriteLine($"Nueva línea detectada: {line}");
                }
                else
                {
                    last++;
                }
            }
        }
    }
}