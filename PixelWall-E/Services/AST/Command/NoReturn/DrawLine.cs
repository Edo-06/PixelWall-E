
public class DrawLine: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}
    public DrawLine(CodeLocation location): base(location)
    {
        size = 3;
        name = "DrawLine";
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
            if(parameters[i].type != ExpressionType.Number)
                return false;
        }
        return true;
    }
    public override async Task Evaluate()
    {
        await PipeLineManager.semaphore.WaitAsync();
        try{if(parameters[0] == null || parameters[1] == null || parameters[2] == null)
            return;
        if(parameters[0].value == null || parameters[1].value == null || parameters[2].value == null)
        {
            Console.WriteLine("Error: null value in parameters");
            return;
        }
            
        int currentX = PipeLineManager.currentPixel.x;
        int currentY = PipeLineManager.currentPixel.y;
        for(int i = 0; i <= (int)parameters[2].value; i++)
        {
            if(i > 0)
            {
                currentX += (int)parameters[0].value;
                currentY += (int)parameters[1].value;
            }
            Console.WriteLine($"Drawing" + PipeLineManager.brushColor + $" at ({currentX}, {currentY})");
            await PipeLineManager.Draw(currentX, currentY, PipeLineManager.brushColor);
        }
        PipeLineManager.currentPixel = (currentX, currentY);
        //await PipeLineManager.DrawWallE();
        }
        finally
        {
            PipeLineManager.semaphore.Release();
        }
    }
}