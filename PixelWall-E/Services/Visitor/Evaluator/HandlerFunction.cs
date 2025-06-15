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
                throw new NotImplementedException($"Function {function.tokenType} is not implemented.");
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
    private static object ExecuteGetColorCount(FunctionNode function)
    {
        return null;
    }
    private static Rgba32 ExecuteGetPixelColor(FunctionNode function)
    {
        return PipeLineManager.GetPixelColor((int)function.parameters[0].value, (int)function.parameters[1].value);
    }
    private static object ExecuteIsBrushColor(FunctionNode function)
    {
        return PincelState.brushColor == (Rgba32)function.parameters[0].value; 
    }
    private static object ExecuteIsBrushSize(FunctionNode function)
    {
        return PincelState.brushSize == (int)function.parameters[0].value;
    }
    private static object ExecuteIsCanvasColor(FunctionNode function)
    {
        return null;
    }

}