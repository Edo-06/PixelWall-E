using PixelWall_E.Components;
public  static class PipeLineManager
{
    private static LexerAnalyzer lexer = new LexerAnalyzer();
    private static List<Token> tokens = new List<Token>();
    public static List<Node> nodes = new List<Node>();
    public static CanvasGrid? canvas;
    public static async Task Start(string code)
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
        
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return;
        }
        await canvas.ChangePixelColor(0,0,"black");
        await canvas.ChangePixelColor(2,2,"red");
        await canvas.ChangePixelColor(2,3,"blue");
        await canvas.ChangePixelColor(3,2,"green");
        await canvas.ChangePixelColor(3,3,"yellow");
        

        await Prove(5,6);
        await Prove(10,15);
        await Prove(25, 30);
        await canvas.ChangePixelColor(0,0,"white");

    }
    public static async Task Prove(int x, int y)
    {
        await canvas.ChangePixelColor(x,y,"red");
    }
}
