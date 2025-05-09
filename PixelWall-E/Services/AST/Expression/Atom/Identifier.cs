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
    public override Task Evaluate()
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
                    return Task.CompletedTask;
                }
            }
        }
        return Task.CompletedTask;
    }
}

public class Variable: Identifier
{
    public override string name { get; set; }
    public override object? value { get; set; }
    public override ExpressionType type {get; set;}

    public Variable(string name, CodeLocation location) : base(location)
    {
        this.name = name;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        return true;
    }
    public override Task Evaluate(){
        if(Scope.variables.ContainsKey(name))
        {
            value = Scope.variables[name].value;
            type = Scope.variables[name].type;
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}
public class Assignament: Identifier
{
    public override string name { get; set; }
    public override object? value { get; set; }
    public override ExpressionType type {get; set;}
    public Expression? expression { get; set; }
    public Assignament(string name, CodeLocation location) : base(location)
    {
        this.name = name;
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        if(expression == null)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Expression is null"));
            return false;
        }
        expression.CheckSemantic(errors);
        type = expression.type;
        return true;
    }
    public override Task Evaluate()
    {
        if(expression != null)
        {
            expression.Evaluate();
            value = expression.value;
            if(Scope.variables.ContainsKey(name))
            {
                Scope.variables[name] = this;
                return Task.CompletedTask; 
            }
            Scope.variables.Add(name, this);
            return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }
}