
public class Color: Command
{
    public override string name { get ; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}
    public Color(CodeLocation location):base(location)
    {
        size = 1;
        name = "Color";
        parameters = new List<Expression?>(size);
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