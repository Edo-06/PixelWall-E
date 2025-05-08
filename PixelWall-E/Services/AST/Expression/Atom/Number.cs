public class Number: Atom
{
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Number;}
        set {}
    }

    public Number(string value, CodeLocation location) : base(location)
    {
        this.value = int.Parse(value);
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {}

}