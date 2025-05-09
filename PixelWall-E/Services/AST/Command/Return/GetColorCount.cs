public class GetColorCount: Function
{
    public override int size {get; set;}
    public override List<Expression?> parameters {get; set;}
    public override string name {get; set;}
    public override Expression? rtn { get; set; }
    public GetColorCount(CodeLocation location): base(location)
    {
        name = "GetColorCount";
        size = 5;
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
        if(!(parameters[0] is ColorString))
            return false;
        for(int i = 1; i < parameters.Count; i++)
        {
            if(!(parameters[i] is Number && parameters[i] is Identifier))
                return false;
        }
        return true;
    }
}