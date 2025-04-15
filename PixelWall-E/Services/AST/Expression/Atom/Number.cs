public class Number: Atom
{
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Number;}
        set {}
    }

    public Number(int value, CodeLocation location) : base(location)
    {
        this.value = value;
    }
    public override bool CheckSemantic()
    {
        return true;
    }
    public override void Evaluate(){}

}