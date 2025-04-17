public class GetCanvasSize: Function
{
    public override int size {get; set;}
    public override List<Expression?> parameters {get; set;}
    public override string name {get; set;}
    public override Expression? rtn { get; set; }
    public GetCanvasSize(CodeLocation location): base(location)
    {
        name = "GetCanvasSize";
        size = 0;
        parameters = new List<Expression?>(size);
    }
    public override void Evaluate()
    {
        throw new NotImplementedException();
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