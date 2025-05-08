
public class GoTo: Command
{
    public override string name {get; set;}
    public override int size { get; set; }
    public override List<Expression?> parameters {get; set;}
    public Label? label {get; set;}
    public GoTo(CodeLocation location):base(location)
    {
        name = "GoTo";
        size = 1;
        parameters = new List<Expression?>();
    }
    public override void Evaluate()
    {
        throw new NotImplementedException();
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        throw new NotImplementedException();
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        throw new NotImplementedException();
    }
}