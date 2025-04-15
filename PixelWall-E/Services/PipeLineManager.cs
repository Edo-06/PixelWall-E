public class PipeLineManager
{
    LexerAnalyzer lexer = new LexerAnalyzer();
    List<Token> tokens = new List<Token>(); 
    public void Start(string code)
    {
        tokens = lexer.GetTokens(code);
    }
}
