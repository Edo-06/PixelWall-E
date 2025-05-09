public class DrawCircle: Command
{
    public override string name { get ; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}


    public DrawCircle(CodeLocation location): base(location)
    {
        size = 3;
        name = "DrawCircle";
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
        PipeLineManager.pixelChange.Add(
                new PipeLineManager.Pixel(PipeLineManager.currentPixel.x, PipeLineManager.currentPixel.y, PipeLineManager.brushColor)
            );
        if (parameters[0] == null || parameters[1] == null || parameters[2] == null)
            return;
        if (parameters[0].value == null || parameters[1].value == null || parameters[2].value == null)
        {
            Console.WriteLine("Error: null value in parameters");
            return;
        }

        int dirX = (int)parameters[0].value;
        int dirY = (int)parameters[1].value;
        int radius = (int)parameters[2].value;

        int startX = PipeLineManager.currentPixel.x + dirX;
        int startY = PipeLineManager.currentPixel.y + dirY;

        int centerX, centerY, dir;
        if(dirX == 1 || dirX == 0)
        {
            centerX = startX +  radius; 
            centerY = startY + radius;
            dir = 1; 
        }
        else
        {
            centerX = startX -  radius; 
            centerY = startY - radius;
            dir = -1;
        }


        List<(int x, int y)> circlePixels = new List<(int x, int y)>();

        DrawCirclePixels(centerX, centerY, radius, circlePixels);
        /* for(int i = 0; i < circlePixels.Count; i++)
        {
            PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(circlePixels[i].x, circlePixels[i].y, PipeLineManager.brushColor));
        } */
        Move(circlePixels, startX, startY, centerX, centerY, dir);
    }
    private void DrawCirclePixels(int centerX, int centerY, int radius, List<(int, int)> circlePixels)
    {
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y < radius * radius && x * x + y * y >= (radius - 1) * (radius - 1))
                {
                    int drawX = centerX + x;
                    int drawY = centerY + y;
                    Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({drawX}, {drawY}) for circle");
                    circlePixels.Add((drawX, drawY));
                }
            }
        }
    }
    private void Move(List<(int x, int y)> circlePixels, int startX, int startY, int centerX, int centerY,int dir)
    {
        int moveY = 1;
        int moveX = 0;
        while(!circlePixels.Contains((moveX + startX,startY + moveY)))
        {
            moveX += dir;
        }
        for(int i = 0; i < circlePixels.Count; i++)
        {
            PipeLineManager.pixelChange.Add(new PipeLineManager.Pixel(circlePixels[i].x - moveX, circlePixels[i].y - moveY, PipeLineManager.brushColor));
        }
        PipeLineManager.currentPixel = (centerX - moveX , centerY - moveY); 
    }
}