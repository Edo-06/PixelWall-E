public class Identifier: Atom
{
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Number;}
        set {}
    }

    public Identifier(int value, CodeLocation location) : base(location)
    {
        this.value = value;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate(){}
}