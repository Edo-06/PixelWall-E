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
    public override void Evaluate()
    {
        if(Scope.labels.ContainsKey(name))
        {
            for(int i = Scope.labels[name]; i < PipeLineManager.nodes[PipeLineManager.nodes.Count - 1].location.line; i++)
            {
                if(PipeLineManager.nodes[i].location.line == Scope.labels[name])
                {
                    for(int j = i; j < PipeLineManager.nodes.Count; j++)
                    {
                        PipeLineManager.nodes[i].Evaluate();
                    }
                    return;
                }
            }
        }
    }
}

public class Variable: Identifier
{
    public override string name { get; set; }
    public override object? value { get; set; }
    public Expression? expression { get; set; }
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
    public override void Evaluate(){
        if(expression != null)
        {
            expression.Evaluate();
            value = expression.value;
        }
        else
        {
            for(int i = location.line; i > 0; i--)
            {
                if(Scope.variables.ContainsKey((name,i)))
                {
                    value = Scope.variables[(name,i)];
                    return;
                }
            }
        }
    }
}