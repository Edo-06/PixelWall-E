public class IsBrushSize: Function
{
    public override int size {get; set;}
    public override List<Expression?> parameters {get; set;}
    public override string name {get; set;}
    public override Expression? rtn { get; set; }
    public IsBrushSize(CodeLocation location): base(location)
    {
        name = "IsBrushSize";
        size = 1;
        parameters = new List<Expression?>(size);
    }
    public override Task Evaluate()
    {
        return Task.CompletedTask;
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
}