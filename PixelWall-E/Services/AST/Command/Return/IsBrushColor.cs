public class IsBrushColor: Function
{
    public override int size {get; set;}
    public override List<Expression?> parameters {get; set;}
    public override string name {get; set;}
    public override Expression? rtn { get; set; }
    public IsBrushColor(CodeLocation location): base(location)
    {
        name = "IsBrushColor";
        size = 1;
        parameters = new List<Expression?>(size);
    }
    public override void Evaluate()
    {
        throw new NotImplementedException();
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return base.Check(errors) && parameters[0] is ColorString;
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        if(parameters[0] is ColorString)
            return true;
        return false;
    }
}