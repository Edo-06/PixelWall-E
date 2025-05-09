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
        if(errors.Count != 0)
            return false;
        return base.Check(errors) && CheckParameters(errors);
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        for(int i = 0; i < parameters.Count; i++)
        {
            if(parameters[i].type != ExpressionType.Number)
                return false;
        }
        return true;
    }
    public override async Task Evaluate()
    {
        await PipeLineManager.semaphore.WaitAsync();
        try{
            PipeLineManager.Draw(PipeLineManager.currentPixel.x, PipeLineManager.currentPixel.y, PipeLineManager.currentPixelColor);
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

            int centerX =startX + dirX * radius; 
            int centerY = startY + dirY * radius; 

            List<(int, int)> circlePixels = new List<(int, int)>();

            DrawCirclePixels(centerX, centerY, radius, circlePixels);
            await Move(circlePixels, startX, startY, centerX, centerY);
        }
        finally
        {
            PipeLineManager.semaphore.Release();
        }
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
    private async Task Move(List<(int x, int y)> circlePixels, int startX, int startY, int centerX, int centerY)
    {
        int moveY = 1;
        int moveX = 0;
        while(!circlePixels.Contains((moveX + startX,startY + moveY)))
        {
            moveX += 1;
        }
        for(int i = 0; i < circlePixels.Count; i++)
        {
            await PipeLineManager.Draw(circlePixels[i].x - moveX, circlePixels[i].y - moveY, PipeLineManager.brushColor);
        }
        PipeLineManager.currentPixel = (centerX - moveX , centerY - moveY); 
        //await PipeLineManager.DrawWallE();
    }
    /*private void DrawX(List<(int, int)> circlePixels, int beginX, int beginY)
    {
        while(circlePixels.Contains((beginX,beginY)))
        {
            beginX += 1;
        }
        DrawY(circlePixels, beginX, beginY);
    }
    private void DrawY(List<(int, int)> circlePixels, int beginX, int beginY)
    {
        while(!circlePixels.Contains((beginX,beginY)))
        {
            beginX += 1;
        }
        while(circlePixels.Contains((beginX,beginY)))
        {
            beginY += 1;
        }
        
    }*/
}