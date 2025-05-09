
public class DrawRectangle: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}

    public DrawRectangle(CodeLocation location): base(location)
    {
        size = 5;
        name = "DrawRectangle";
        parameters = new List<Expression?>(size);
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return base.Check(errors) && CheckParameters(errors);
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        for(int i = 0; i < parameters.Count; i++)
        {
            if(!(parameters[i] is Number && parameters[i] is Identifier))
                return false;
        }
        return true;
    }
    public override void Evaluate()
    {
        if (parameters[0] == null || parameters[1] == null || parameters[2] == null || parameters[3] == null || parameters[4] == null)
            return;
            if (parameters[0].value == null || parameters[1].value == null || parameters[2].value == null || parameters[3].value == null || parameters[4].value == null)
            {
                Console.WriteLine("Error: null value in parameters");
                return;
            }

            int dirX = (int)parameters[0].value;
            int dirY = (int)parameters[1].value;
            int distance = (int)parameters[2].value;
            int width = (int)parameters[3].value;
            int height = (int)parameters[4].value;

            int startX = PipeLineManager.currentPixel.x + dirX * distance;
            int startY = PipeLineManager.currentPixel.y + dirY * distance;

            int endX = startX + width;
            int endY = startY + height - 1;

            int centerX = (startX + endX) / 2;
            int centerY = (startY + endY) / 2;

            // Dibujar las 4 líneas del rectángulo
            for (int x = startX; x <= endX; x++)
            {
                Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({x}, {startY}) for rectangle");
                PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(x, startY, PipeLineManager.brushColor));
                Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({x}, {endY}) for rectangle");
                PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(x, endY, PipeLineManager.brushColor));
            }

            for (int y = startY + 1; y < endY; y++) // Evitar redibujar las esquinas
            {
                Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({startX}, {y}) for rectangle");
                PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(startX, y, PipeLineManager.brushColor));
                Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({endX}, {y}) for rectangle");
                PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(endX, y, PipeLineManager.brushColor));
            }
            PipeLineManager.currentPixel = (centerX, centerY);
    }
}