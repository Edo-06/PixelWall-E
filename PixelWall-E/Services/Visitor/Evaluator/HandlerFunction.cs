using SixLabors.ImageSharp.PixelFormats;
public static class HandlerFunction
{
    public static object Execute(FunctionNode function)
    {
        switch(function.tokenType)
        {
            case TokenType.GetActualX:
                return ExecuteGetActualX(function);
            case TokenType.GetActualY:
                return ExecuteGetActualY(function);
            case TokenType.GetCanvasSize:
                return ExecuteGetCanvasSize(function);
            case TokenType.GetColorCount:
                return ExecuteGetColorCount(function);
            case TokenType.IsBrushColor:
                return ExecuteIsBrushColor(function);
            case TokenType.IsBrushSize:
                return ExecuteIsBrushSize(function);
            case TokenType.IsCanvasColor:
                return ExecuteIsCanvasColor(function);
            default:
                throw new RuntimeError(function.location, RuntimeErrorCode.NotImplemented, $".Unknown command type: '{function.tokenType}'.");
        }
    }
    private static int ExecuteGetActualX(FunctionNode function)
    {
        return PipeLineManager.currentPixel.x;
    }
    private static int ExecuteGetActualY(FunctionNode function)
    {
        return PipeLineManager.currentPixel.y;
    }
    private static int ExecuteGetCanvasSize(FunctionNode function)
    {
        return PipeLineManager.GetCanvasSize();
    }
    private static int ExecuteGetColorCount(FunctionNode function)
    {
        Rgba32 color = (Rgba32)function.parameters[0].value;
        int x1 = (int)function.parameters[1].value;
        int y1 = (int)function.parameters[2].value;
        int x2 = (int)function.parameters[3].value;
        int y2 = (int)function.parameters[4].value;
        CheckBounds(x1,y1,function);
        CheckBounds(x2,y2,function);
        
        int minX = Math.Min(x1,x2);
        int minY = Math.Min(y1,y2);
        int maxX = Math.Max(x1,x2);
        int maxY = Math.Max(y1,y2);

        int count = 0;
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (PipeLineManager.GetPixelColor(x,y) == color)
                {
                    count++;
                }
            }
        }
        return count;
    }
    private static Rgba32 ExecuteGetPixelColor(FunctionNode function)
    {
        int x = (int)function.parameters[0].value;
        int y = (int)function.parameters[1].value;
        CheckBounds(x,y,function);
        return PipeLineManager.GetPixelColor(x, y);
    }
    private static bool ExecuteIsBrushColor(FunctionNode function)
    {
        return PincelState.brushColor == (Rgba32)function.parameters[0].value; 
    }
    private static bool ExecuteIsBrushSize(FunctionNode function)
    {
        return PincelState.brushSize == (int)function.parameters[0].value;
    }
    private static bool ExecuteIsCanvasColor(FunctionNode function)
    {
        Rgba32 color = (Rgba32)function.parameters[0].value;
        int x = (int)function.parameters[1].value;
        int y = (int)function.parameters[2].value;

        CheckBounds(x,y,function);
        return PipeLineManager.GetPixelColor(x,y) == color;
    }

    private static void CheckBounds(int x, int y, FunctionNode function)
    {
        int canvasSize = PipeLineManager.GetCanvasSize();
        if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
        {
            throw new RuntimeError(function.location, RuntimeErrorCode.OutOfBounds,
                $"pixel ({x}, {y}) is out of canvas bounds (0-{canvasSize - 1}, 0-{canvasSize - 1}).");
        }
    }
}