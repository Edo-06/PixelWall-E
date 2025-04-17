public class ColorString: Atom
{
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Color;}
        set {}
    }

    public ColorString(CodeLocation location) : base(location)
    {}
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate(){}
}