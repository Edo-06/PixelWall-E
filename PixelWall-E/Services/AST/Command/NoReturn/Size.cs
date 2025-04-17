
public class Size: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}

    public Size(CodeLocation location): base(location)
    {
        size = 1;
        name = "Size";
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
}