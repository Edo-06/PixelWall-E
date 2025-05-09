public class GetActualX: Function
{
    public override int size {get; set;}
    public override List<Expression?> parameters {get; set;}
    public override string name {get; set;}
    public override Expression? rtn { get; set; }
    public GetActualX(CodeLocation location): base(location)
    {
        name = "GetActualX";
        size = 0;
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
        return true;
    }
}