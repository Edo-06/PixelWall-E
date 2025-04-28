
public class DrawRectangle: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}

    public DrawRectangle(CodeLocation location): base(location)
    {
        size = 5;
        name = "DrawRectangle";
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
        throw new NotImplementedException();
    }
}