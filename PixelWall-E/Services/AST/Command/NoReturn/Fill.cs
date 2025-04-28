public class Fill: Command
{
    public override string name { get; set; }
    public override List<Expression?> parameters {get; set;}
    public override int size {get; set;}
    public Fill(CodeLocation location) : base(location) 
    {
        size = 0;
        name = "Fill";
        parameters = new List<Expression?>(size);
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return base.Check(errors) && CheckParameters(errors);
    }
    public override bool CheckParameters(List<CompilingError> errors)
    {
        return true;
    }
        public override void Evaluate()
    {
        throw new NotImplementedException();
    }
}