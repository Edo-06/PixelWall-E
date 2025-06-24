using SixLabors.ImageSharp.PixelFormats;

public static class HandlerCommand
{
    public static async Task Execute(CommandNode command)
    {
        switch (command.tokenType)
        {
            case TokenType.Spawn:
                ExecuteSpawn(command);
                break;
            case TokenType.MoveTo:
                ExecuteSpawn(command);
                break;
            case TokenType.DrawLine:
                await ExecuteDrawLine(command);
                break;
            case TokenType.DrawCircle:
                await ExecuteDrawCircle(command);
                break;
            case TokenType.DrawRectangle:
                await ExecuteDrawRectangle(command);
                break;
            case TokenType.Fill:
                await ExecuteFill(command);
                break;
            case TokenType.Color:
                ExecuteColor(command);
                break;
            case TokenType.Size:
                ExecuteSize(command);
                break;
            default:
                throw new RuntimeError(command.location, RuntimeErrorCode.NotImplemented, $".Unknown command type: '{command.tokenType}'.");
        }
    }
    private static void ExecuteSpawn(CommandNode command)
    {
        int x = (int)command.parameters[0].value;
        int y = (int)command.parameters[1].value;

        CheckBounds(x, y, command);
        PipeLineManager.currentPixel = (x, y);
        Console.WriteLine($"Spawning at ({x}, {y})");
    }
    private static async Task ExecuteDrawLine(CommandNode command)
    {
        int distance = (int)command.parameters[2].value;
        Console.WriteLine($"linea de {distance}");

        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;
        ValidateDirection(dirX, dirY, command);

        int currentX = PipeLineManager.currentPixel.x;
        int currentY = PipeLineManager.currentPixel.y;
        CheckBounds(currentX, currentY, command);

        PincelState.PaintBrushAt(currentX, currentY);
        if (isAwait) await Task.Delay(1);
        for (int i = 0; i < distance; i++)
        {
            if (!PipeLineManager.isRunning) return;

            currentX += dirX;
            currentY += dirY;
            CheckBounds(currentX, currentY, command);
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({currentX}, {currentY})");
            PincelState.PaintBrushAt(currentX, currentY);
            if (isAwait) await Task.Delay(1);
        }
        PipeLineManager.currentPixel = (currentX, currentY);
    }
    private static async Task ExecuteDrawCircle(CommandNode command)
    {

        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;
        ValidateDirection(dirX, dirY, command);

        int radius = (int)command.parameters[2].value;

        int centerX = PipeLineManager.currentPixel.x + dirX * radius;
        int centerY = PipeLineManager.currentPixel.y + dirY * radius;

        CheckBounds(centerX, centerY, command);

        PipeLineManager.currentPixel = (centerX, centerY);
        if (radius < 5)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (!PipeLineManager.isRunning) return;
                    if (x * x + y * y <= radius * radius && x * x + y * y > (radius - 1) * (radius - 1))
                    {
                        int currentX = centerX + x;
                        int currentY = centerY + y;
                        Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({currentX}, {currentY}) for circle");
                        PincelState.PaintBrushAt(currentX, currentY);
                        if (isAwait) await Task.Delay(1);
                    }
                }
            }
        }

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (!PipeLineManager.isRunning) return;
                if (x * x + y * y < radius * radius && x * x + y * y >= (radius - 1) * (radius - 1))
                {
                    int currentX = centerX + x;
                    int currentY = centerY + y;
                    Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({currentX}, {currentY}) for circle");
                    PincelState.PaintBrushAt(currentX, currentY);
                    if (isAwait) await Task.Delay(1);
                }
            }
        }
    }
    private static async Task ExecuteDrawRectangle(CommandNode command)
    {
        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;
        ValidateDirection(dirX, dirY, command);

        int distance = (int)command.parameters[2].value;
        int width = (int)command.parameters[3].value;
        int height = (int)command.parameters[4].value;

        int centerX = PipeLineManager.currentPixel.x + dirX * distance;
        int centerY = PipeLineManager.currentPixel.y + dirY * distance;
        CheckBounds(centerX, centerY, command);

        int startX = centerX - width / 2;
        int startY = centerY - height / 2;

        int endX = centerX + width/2 ;
        int endY = centerY + height/2;

        for (int x = startX; x <= endX; x++)
        {
            if (!PipeLineManager.isRunning) return;
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({x}, {startY}) for rectangle");
            PincelState.PaintBrushAt(x, startY);
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({x}, {endY}) for rectangle");
            PincelState.PaintBrushAt(x, endY);
            if (isAwait) await Task.Delay(1);
        }

        for (int y = startY + 1; y < endY; y++)
        {
            if (!PipeLineManager.isRunning) return;
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({startX}, {y}) for rectangle");
            PincelState.PaintBrushAt(startX, y);
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({endX}, {y}) for rectangle");
            PincelState.PaintBrushAt(endX, y);
            if (isAwait) await Task.Delay(1);
        }
        PipeLineManager.currentPixel = (centerX, centerY);
    }
    private static async Task ExecuteFill(CommandNode command)
    {
        int startX = PipeLineManager.currentPixel.x;
        int startY = PipeLineManager.currentPixel.y;
        if (PipeLineManager.currentPixel.x < 0 || PipeLineManager.currentPixel.y < 0 ||
        PipeLineManager.currentPixel.x >= PipeLineManager.GetCanvasSize() ||
        PipeLineManager.currentPixel.y >= PipeLineManager.GetCanvasSize())
        {
            Console.WriteLine("Error: Current pixel is out of bounds.");
            return;
        }
        Rgba32 targetColor = PipeLineManager.GetPixelColor(PipeLineManager.currentPixel.x, PipeLineManager.currentPixel.y);
        Rgba32 replacementColor = PincelState.brushColor;

        if (targetColor == replacementColor) return;

        //Start
        Queue<(int x, int y)> pixelsToVisit = new Queue<(int x, int y)>();
        pixelsToVisit.Enqueue((startX, startY));

        HashSet<(int x, int y)> visitedPixels = new HashSet<(int x, int y)>();
        visitedPixels.Add((startX, startY));

        int pixelsProcessed = 0;
        int delayBatchSize = 50;

        while (pixelsToVisit.Any())
        {
            (int currentX, int currentY) = pixelsToVisit.Dequeue();

            if (!PipeLineManager.isRunning) return;
            PincelState.PaintBrushAt(currentX, currentY);
            pixelsProcessed++;
            if (pixelsProcessed % delayBatchSize == 0)
            {
                if (isAwait) await Task.Delay(1);
            }

            (int dx, int dy)[] directions = { (0, 1), (0, -1), (1, 0), (-1, 0) };

            foreach (var (dx, dy) in directions)
            {
                int neighborX = currentX + dx;
                int neighborY = currentY + dy;

                if (neighborX < 0 || neighborX >= PipeLineManager.GetCanvasSize() ||
                    neighborY < 0 || neighborY >= PipeLineManager.GetCanvasSize())
                {
                    continue; // Skip out of bounds neighbors
                }
                if (!visitedPixels.Contains((neighborX, neighborY)))
                {
                    if (PipeLineManager.GetPixelColor(neighborX, neighborY) == targetColor)
                    {
                        pixelsToVisit.Enqueue((neighborX, neighborY));
                        visitedPixels.Add((neighborX, neighborY));
                    }
                }
            }
        }
        Console.WriteLine("Relleno completado.");
    }
    private static void ExecuteColor(CommandNode command)
    {
        PincelState.SetBrushColor((Rgba32)command.parameters[0].value);
        Console.WriteLine($"Brush color set to {PincelState.brushColor}");
    }
    private static void ExecuteSize(CommandNode command)
    {
        int size = (int)command.parameters[0].value;
        if (size < 1 || size > PipeLineManager.GetCanvasSize() / 4)
            throw new RuntimeError(command.location, RuntimeErrorCode.OutOfBounds,
                $"brush size must be in range (1-{PipeLineManager.GetCanvasSize() / 4})");
        PincelState.SetBrushSize(size);
        Console.WriteLine($"Brush size set to {PincelState.brushSize}");
    }
    #region Checks
    private static void CheckBounds(int x, int y, CommandNode command)
    {
        int canvasSize = PipeLineManager.GetCanvasSize();
        if (x < 0 || x >= canvasSize || y < 0 || y >= canvasSize)
            throw new RuntimeError(command.location, RuntimeErrorCode.OutOfBounds,
                $"pixel ({x}, {y}) is out of canvas bounds (0-{canvasSize - 1}, 0-{canvasSize - 1}).");
    }
    private static void ValidateDirection(int dx, int dy, CommandNode command)
    {
        bool isValidDirection =
            (dx == -1 && (dy == -1 || dy == 0 || dy == 1)) ||
            (dx == 0 && (dy == -1 || dy == 1 || dy == 0)) ||
            (dx == 1 && (dy == -1 || dy == 0 || dy == 1));

        if (!isValidDirection)

            throw new RuntimeError(command.location, RuntimeErrorCode.InvalidDirection,
                $"({dx}, {dy}). Direction components must be -1, 0, or 1, and not both zero.");
    }
    public static bool isAwait = true;
    public static void ReStart()
    {
        isAwait = true;
    }
    #endregion
}