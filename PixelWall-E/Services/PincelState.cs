public static class PincelState
{
    public static int brushSize { get; private set; } = 1;
    public static string brushColor {get; private set; } = "transparent";

    public static void SetBrushSize(int k)
    {
        if (k <= 0)
        {
            Console.WriteLine("Error: El tamaño del pincel debe ser un número positivo.");
            return;
        }

        if (k % 2 == 0)
        {
            brushSize = k - 1;
        }
        else 
        {
            brushSize = k;
        }
        Console.WriteLine($"Tamaño del pincel actualizado a: {brushSize}");
    }
    public static void SetBrushColor(string color)
    {
        brushColor = color;
    }

    public static async Task PaintBrushAt(int centerX, int centerY)
    {
        if(brushColor == "transparent") return;

        int offset = (brushSize - 1) / 2;

        for (int dx = -offset; dx <= offset; dx++)
        {
            for (int dy = -offset; dy <= offset; dy++)
            {
                int pixelX = centerX + dx;
                int pixelY = centerY + dy;

                PipeLineManager.ChangePixelColor(pixelX, pixelY, brushColor);
            }
        }
        await Task.Delay(1);
    }
    public static void ReStart()
    {
        brushSize = 1;
        brushColor = "transparent";
    }
}