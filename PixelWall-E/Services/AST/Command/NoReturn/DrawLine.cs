
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
            if(!(parameters[i] is Number && parameters[i] is Identifier))
                return false;
        }
        return true;
    }
    public override void Evaluate()
    {
        if(parameters[0] == null || parameters[1] == null || parameters[2] == null)
            return;
        parameters[0].Evaluate();
        parameters[1].Evaluate();
        parameters[2].Evaluate();
        if(parameters[0].value == null || parameters[1].value == null || parameters[2].value == null)
        {
            Console.WriteLine("Error: null value in parameters");
            return;
        }
            
        int currentX = PipeLineManager.currentPixel.x;
        int currentY = PipeLineManager.currentPixel.y;
        for(int i = 0; i < (int)parameters[2].value; i++)
        {
            currentX += (int)parameters[0].value;
            currentY += (int)parameters[1].value;
            Console.WriteLine($"Drawing" + PipeLineManager.currentColor + $" at ({currentX}, {currentY})");
            PipeLineManager.pixelChange.Add(
                new PipeLineManager.Pixel(currentX, currentY, PipeLineManager.currentColor)
            );
        }
        PipeLineManager.currentPixel = (currentX, currentY);
    }
}