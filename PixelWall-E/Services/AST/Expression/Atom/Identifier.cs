public abstract class Identifier: Atom
{
    public abstract string name { get; set; }
    public Identifier(CodeLocation location) : base(location){}
}
public class Label: Identifier
{
    public override string name { get; set; }
    public override object? value { get; set; }
    public override ExpressionType type
    {
        get{return ExpressionType.Label;}
        set {}
    }

    public Label(string name, CodeLocation location) : base(location)
    {
        this.name = name;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate(){}
}

public class Variable: Identifier
{
    public override string name { get; set; }
    public override object? value { get; set; }
    public override ExpressionType type 
    {
        get{return ExpressionType.Number;}
        set {}
    }

    public Variable(string name, CodeLocation location) : base(location)
    {
        this.name = name;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate(){}
}