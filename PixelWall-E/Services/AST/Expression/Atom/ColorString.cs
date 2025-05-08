public class ColorString: Atom
{
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Color;}
        set {}
    }

    public ColorString(CodeLocation location, string value) : base(location)
    {   
        string cleaned = value.Replace("\"", "").Trim();
        this.value = cleaned;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate(){}
}