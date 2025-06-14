public static class HandlerCommand
{
    public static async Task Execute(CommandNode command)
    {
        switch (command.tokenType)
        {
            case TokenType.Spawn:
                await ExecuteSpawn(command);
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
        }
    }
    private static async Task ExecuteSpawn(CommandNode command)
    {
        int x = (int)command.parameters[0].value;
        int y = (int)command.parameters[1].value;

        PipeLineManager.currentPixel = (x, y);
        await PincelState.PaintBrushAt(x,y);
    }
    private static async Task ExecuteDrawLine(CommandNode command)
    {
        int distance = (int)command.parameters[2].value;
        Console.WriteLine($"linea de {distance}");
        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;

        int currentX = PipeLineManager.currentPixel.x;
        int currentY = PipeLineManager.currentPixel.y;

        for(int i = 0; i < distance; i++)
        {
            if(!PipeLineManager.isRunning) return;
                currentX += dirX;
                currentY += dirY;
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({currentX}, {currentY})");
            await PincelState.PaintBrushAt(currentX, currentY);
        }
        PipeLineManager.currentPixel = (currentX, currentY);
    }
    private static async Task ExecuteDrawCircle(CommandNode command)
    {

        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;
        int radius = (int)command.parameters[2].value;

        int centerX = PipeLineManager.currentPixel.x + dirX*radius;
        int centerY = PipeLineManager.currentPixel.y + dirY*radius;

        PipeLineManager.currentPixel = (centerX, centerY);
        
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if(!PipeLineManager.isRunning) return;
                if (x * x + y * y < radius * radius && x * x + y * y >= (radius - 1) * (radius - 1))
                {
                    int currentX = centerX + x;
                    int currentY = centerY + y;
                    Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({currentX}, {currentY}) for circle");
                    await PincelState.PaintBrushAt(currentX, currentY);
                }
            }
        }
        //await PincelState.PaintBrushAt(PipeLineManager.currentPixel.x, PipeLineManager.currentPixel.y);
    }
    private static async Task ExecuteDrawRectangle(CommandNode command)
    {
        int dirX = (int)command.parameters[0].value;
        int dirY = (int)command.parameters[1].value;
        int distance = (int)command.parameters[2].value;
        int width = (int)command.parameters[3].value;
        int height = (int)command.parameters[4].value;

        int startX = PipeLineManager.currentPixel.x + dirX * distance;
        int startY = PipeLineManager.currentPixel.y + dirY * distance;

        int endX = startX + width;
        int endY = startY + height - 1;

        int centerX = (startX + endX) / 2;
        int centerY = (startY + endY) / 2;

        for (int x = startX; x <= endX; x++)
        {
            if(!PipeLineManager.isRunning) return;
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({x}, {startY}) for rectangle");
            await PincelState.PaintBrushAt(x, startY);
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({x}, {endY}) for rectangle");
            await PincelState.PaintBrushAt(x, endY);
        }

        for (int y = startY + 1; y < endY; y++) 
        {
            if(!PipeLineManager.isRunning) return;
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({startX}, {y}) for rectangle");
            await PincelState.PaintBrushAt(startX, y);
            Console.WriteLine($"Drawing" + PincelState.brushColor + $" at ({endX}, {y}) for rectangle");
            await PincelState.PaintBrushAt(endX, y);
        }
        PipeLineManager.currentPixel = (centerX, centerY);
        //await PincelState.PaintBrushAt(centerX, centerY);
    }
    private static async Task ExecuteFill(CommandNode command)
    {
        int startX = PipeLineManager.currentPixel.x;
        int startY = PipeLineManager.currentPixel.y;
        string targetColor = PipeLineManager.GetPixelColor(PipeLineManager.currentPixel.x, PipeLineManager.currentPixel.y);
        string replacementColor = PincelState.brushColor; 

        if (targetColor == replacementColor) return;

        int size = PipeLineManager.GetCanvasSize();

        //Start
        Queue<(int x, int y)> pixelsToVisit = new Queue<(int x, int y)>();
        pixelsToVisit.Enqueue((startX, startY));

        HashSet<(int x, int y)> visitedPixels = new HashSet<(int x, int y)>();
        visitedPixels.Add((startX, startY));

        while (pixelsToVisit.Any())
        {
            (int currentX, int currentY) = pixelsToVisit.Dequeue();

            PipeLineManager.currentPixel = (currentX, currentY);
            if(!PipeLineManager.isRunning) return;
            await PincelState.PaintBrushAt(currentX, currentY);

            (int dx, int dy)[] directions = { (0, 1), (0, -1), (1, 0), (-1, 0) };

            foreach (var (dx, dy) in directions)
            {
                int neighborX = currentX + dx;
                int neighborY = currentY + dy;

                if (neighborX >= 0 && neighborX < size &&
                    neighborY >= 0 && neighborY < size)
                {
                    if (!visitedPixels.Contains((neighborX, neighborY)))
                    {
                        if(PipeLineManager.GetPixelColor(neighborX, neighborY) == targetColor)
                        {
                            pixelsToVisit.Enqueue((neighborX, neighborY));
                            visitedPixels.Add((neighborX, neighborY));
                        }
                    }
                }
            }
        }
        Console.WriteLine("Relleno completado.");
    }
    private static void ExecuteColor(CommandNode command)
    {
        PincelState.SetBrushColor((string)command.parameters[0].value);
        Console.WriteLine($"Brush color set to {PincelState.brushColor}");
    }
    private static void ExecuteSize(CommandNode command)
    {
        PincelState.SetBrushSize((int)command.parameters[0].value);
        Console.WriteLine($"Brush size set to {PincelState.brushSize}");
    }
}