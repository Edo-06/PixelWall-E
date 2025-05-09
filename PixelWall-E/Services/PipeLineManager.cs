using PixelWall_E.Components;
using SixLabors.ImageSharp.PixelFormats;
public  static class PipeLineManager
{
    private static LexerAnalyzer lexer = new LexerAnalyzer();
    private static List<Token> tokens = new List<Token>();
    public static List<Node> nodes = new List<Node>();
    public static CanvasGrid canvas {get; set;} = null!;
    public static (int x, int y) currentPixel {get; set;}= (-1, -1);
    public static string brushColor = "white";
    public static string currentPixelColor = "white";
    public static List<Pixel> pixelChange = new List<Pixel>();
    public static int size {get; set;} = 0;
    public static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private static Parser parser {get; set; } = null!;
    public static async Task Start(string code)
    {
        tokens = lexer.GetTokens(code);
        for(int i = 0; i < tokens.Count; i++)
        {
            Console.WriteLine(tokens[i].type);
        }
        parser = new Parser(tokens);
        for(int i = 0; i < parser.errors.Count; i++)
        {
            Console.WriteLine(parser.errors[i].message);
        }
        if(parser.errors.Count == 0)
        {
            await Interpeter();
        }
        else
        {
            Console.WriteLine("Errors found in the code");
            return;
        }
    }
    public static async Task Draw(int x, int y, string color)
    {
        await canvas.ChangePixelColor(x, y, "Black");
        await canvas.ChangePixelColor(x, y, color);
    }
    /*public static async Task DrawWallE()
    {
        if(currentPixel != (-1, -1))
        {
            currentPixelColor = canvas.GetPixelColor(currentPixel.x, currentPixel.y);
            await canvas.ChangePixelColor(currentPixel.x, currentPixel.y, "Black");
        }
    }*/
    private static async Task Interpeter()
    {
        await Reset();
        parser.programNode.CheckSemantic(parser.errors);
        await parser.programNode.Evaluate();
        Console.WriteLine("currentPixel: " + currentPixel.x + " " + currentPixel.y);
    }
    private static async Task Reset()
    {
        currentPixel = (-1, -1);
        pixelChange = new List<Pixel>();
        size = canvas.numberOfPixels;
    }
    public static string GetPixelColor(int x, int y)
    {
        return canvas.GetPixelColor(x, y);
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
