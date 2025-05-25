using PixelWall_E.Components;
public  static class PipeLineManager
{
    private static LexerAnalyzer lexer = new LexerAnalyzer();
    private static List<Token> tokens = new List<Token>();
    public static ProgramNode program = null!;
    public static CanvasGrid? canvas;
    public static (int x, int y) currentPixel {get; set;}= (0,0);
    public static string brushColor = "white";
    public static string currentPixelColor = "white";
    public static int brushSize;
    public static async Task Start(string code)
    {
        tokens = lexer.GetTokens(code);
        for(int i = 0; i < tokens.Count; i++)
        {
            Console.WriteLine(tokens[i].type);
        }
        Parser parser = new Parser(tokens);
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return;
        }
        SemanticCheckerVisitor semanticChecker = new SemanticCheckerVisitor(parser.errors);
        if(semanticChecker.errors.Count > 0)
        {
            PrintErrors(semanticChecker.errors);
            return;
        }
        semanticChecker.Visit(program);
        if(semanticChecker.errors.Count > 0) 
        {
            PrintErrors(semanticChecker.errors);
            return;
        }
        Executor executor = new Executor();
        await executor.Visit(program);
        /* parser.programNode.CheckSemantic(parser.errors);
        parser.programNode.Evaluate(); */
        Console.WriteLine("currentPixel: " + currentPixel.x + " " + currentPixel.y);
        await Paint();
    }
    public static async Task Paint()
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return;
        }
        /* for(int i = 0; i < pixelChange.Count; i++)
        {
            await canvas.ChangePixelColor(pixelChange[i].x, pixelChange[i].y, "Black");
            await canvas.ChangePixelColor(pixelChange[i].x, pixelChange[i].y, pixelChange[i].color);
        }
        if(currentPixel != (-1,1))
        {
            currentPixelColor = canvas.GetPixelColor(currentPixel.x, currentPixel.y);
            await canvas.ChangePixelColor(currentPixel.x, currentPixel.y, "Black");
        }*/
        await canvas.ChangePixelColor(currentPixel.x, currentPixel.y, "Black"); 
    }
    public static int GetCanvasSize()
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return 0;
        }
        return canvas.numberOfPixels;
    }
    public static string GetPixelColor(int x, int y)
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return "";
        }
        return canvas.GetPixelColor(x, y);
    }
    public static async Task ChangePixelColor(int x, int y)
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return;
        }
        await canvas.ChangePixelColor(x, y, brushColor);
    }
    private static void PrintErrors(List<CompilingError> errors) 
    {
        for(int i = 0; i < errors.Count; i++)
        {
            Console.WriteLine(errors[i].message);
        }
    }
    public struct Pixel
    {
        public int x { get; set; }
        public int y { get; set; }
        public string color { get; set; }
        public Pixel(int x, int y, string color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }
}
