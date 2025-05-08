public class Spawn : Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}

    public Spawn(CodeLocation location): base(location)
    {
        size = 2;
        name = "Spawn";
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
            if(parameters[i] == null)
                return false;
            if(!parameters[i].CheckSemantic(errors))
                return false;
            if(!(parameters[i] is Number && parameters[i] is Variable))
                return false;
        }
        return true;
    }
    public override void Evaluate()
    {
        if(parameters[0] == null || parameters[1] == null)
            return;
        parameters[0].Evaluate();
        parameters[1].Evaluate();
        if(parameters[0].value == null || parameters[1].value == null)
            return;
        PipeLineManager.currentPixel = ((int)parameters[0].value,(int)parameters[1].value);
        PipeLineManager.pixelChange = [];
    }
}