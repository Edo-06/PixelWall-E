using PixelWall_E.Components;
using SixLabors.ImageSharp.PixelFormats;
public  static class PipeLineManager
{
    public delegate Task OnErrorsDetectedEventHandler(Exception errors);
    public static event OnErrorsDetectedEventHandler OnErrorsDetected = delegate {return Task.CompletedTask;};
    private static LexerAnalyzer lexer = new LexerAnalyzer();
    private static List<Token> tokens = new List<Token>();
    public static ProgramNode program = null!;
    public static List<string> lexerExceptions = [];
    public static CanvasGrid? canvas;
    public static (int x, int y) currentPixel {get; set;}= (0,0);
    public static bool isRunning = false;
#region Run
    public static async Task Start(string code)
    {
        try
        {
            tokens = lexer.GetTokens(code);
        }
        catch (LexerException ex)
        {
            lexerExceptions.Add(ex.Message);
            await OnErrorsDetected.Invoke(ex);
            tokens = []; 
        }
        if(lexerExceptions.Count > 0) return;
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
            foreach (var error in semanticChecker.errors)
            {
                await OnErrorsDetected.Invoke(error);
            }
            PrintErrors(semanticChecker.errors);
            return;
        }
        semanticChecker.Visit(program);
        if(semanticChecker.errors.Count > 0) 
        {
            foreach (var error in semanticChecker.errors)
            {
                await OnErrorsDetected.Invoke(error);
            }
            PrintErrors(semanticChecker.errors);
            return;
        }
        Executor executor = new Executor();
        isRunning = true;
        try
        {
            await executor.Visit(program);
        }
        catch(RuntimeError ex)
        {
            Console.WriteLine("Executor Exception: " + ex.Message);
            await OnErrorsDetected.Invoke(ex);
            isRunning = false;
            return;
        }
        Console.WriteLine("currentPixel: " + currentPixel.x + " " + currentPixel.y);
    }
    public static void ReStart()
    {
        isRunning = false;
        lexer = new LexerAnalyzer();
        program = null!;
        tokens = new List<Token>();
        currentPixel = (0,0);
        Scope.variables = [];
        Scope.labels = [];
        PincelState.ReStart();
    }
#endregion
#region Auxiliar Functions
    public static int GetCanvasSize()
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return 0;
        }
        return canvas.numberOfPixels;
    }
    public static Rgba32 GetPixelColor(int x, int y)
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return new Rgba32();
        }
        return canvas.GetPixelColor(x, y);
    }
    public static void ChangePixelColor(int x, int y, Rgba32 color)
    {
        if(canvas == null)
        {
            Console.WriteLine("Canvas is null");
            return;
        }
        canvas.ChangePixelColor(x, y, color);
    }
    private static void PrintErrors(List<Exception> errors) 
    {
        for(int i = 0; i < errors.Count; i++)
        {
            Console.WriteLine(errors);
        }
    }
#endregion
}
