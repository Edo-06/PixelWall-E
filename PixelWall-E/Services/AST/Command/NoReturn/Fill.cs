using SixLabors.ImageSharp.PixelFormats;

public class Fill: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}
    public Fill(CodeLocation location) : base(location) 
    {
        size = 0;
        name = "Fill";
        parameters = new List<Expression?>(size);
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return base.Check(errors) && CheckParameters(errors);
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        return true;
    }
    public override async Task Evaluate()
    {
        await PipeLineManager.semaphore.WaitAsync();
        try
        {
            int startX = PipeLineManager.currentPixel.x;
            int startY = PipeLineManager.currentPixel.y;

            string color = PipeLineManager.GetPixelColor(startX,startY);//PipeLineManager.currentPixelColor;

            Queue<(int x, int y)> queue = new Queue<(int, int)>();

            PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(startX, startY, PipeLineManager.brushColor));
            queue.Enqueue((startX, startY));

            (int dx, int dy)[] directions = new (int, int)[]
            {
                (0, 1),  
                (0, -1), 
                (1, 0),  
                (-1, 0) 
            };

            while (queue.Count > 0)
            {
                (int currentX, int currentY) = queue.Dequeue();

                foreach (var dir in directions)
                {
                    int nextX = currentX + dir.dx;
                    int nextY = currentY + dir.dy;

                    if (IsValidPosition(nextX, nextY))
                    {
                        if (PipeLineManager.GetPixelColor(nextX, nextY).Equals(color))
                        {
                            await PipeLineManager.Draw(nextX, nextY, PipeLineManager.brushColor);
                            queue.Enqueue((nextX, nextY));
                        }
                    }
                }
            }
            //await PipeLineManager.DrawWallE();
        }
        finally
        {
            PipeLineManager.semaphore.Release();
        }
    }
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < PipeLineManager.size && y >= 0 && y < PipeLineManager.size;
    }
}