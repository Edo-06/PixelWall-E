using PixelWall_E.Components;
public  static class PipeLineManager
{
    private static LexerAnalyzer lexer = new LexerAnalyzer();
    private static List<Token> tokens = new List<Token>();
    public static List<Node> nodes = new List<Node>();
    public static CanvasGrid? canvas;
    public static (int x, int y) currentPixel = (0,0);
    public static string brushColor = "white";
    public static string currentPixelColor = "white";
    public static List<Pixel> pixelChange = new List<Pixel>();
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
        parser.programNode.CheckSemantic(parser.errors);
        parser.programNode.Evaluate();
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
        for(int i = 0; i < pixelChange.Count; i++)
        {
            await canvas.ChangePixelColor(pixelChange[i].x, pixelChange[i].y, "Black");
            await canvas.ChangePixelColor(pixelChange[i].x, pixelChange[i].y, pixelChange[i].color);
        }
        if(currentPixel != (-1,1))
        {
            currentPixelColor = canvas.GetPixelColor(currentPixel.x, currentPixel.y);
            await canvas.ChangePixelColor(currentPixel.x, currentPixel.y, "Black");
        }
        await canvas.ChangePixelColor(currentPixel.x, currentPixel.y, "Black");
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
