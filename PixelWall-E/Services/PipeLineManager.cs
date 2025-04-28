public class PipeLineManager
{
    LexerAnalyzer lexer = new LexerAnalyzer();
    List<Token> tokens = new List<Token>();

    public void Start(string code)
    {
        tokens = lexer.GetTokens(code);
        for(int i = 0; i < tokens.Count; i++)
        {
            Console.WriteLine(tokens[i].type);
        }
        Parser parser = new Parser(tokens);
        for(int i = 0; i < parser.errors.Count; i++)
        {
            Console.WriteLine(parser.errors[i].message);
        }
    }
}
