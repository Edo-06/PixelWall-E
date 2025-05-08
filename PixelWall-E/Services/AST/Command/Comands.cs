public abstract class Command: Node
{
    public abstract string name {get; set;}
    public abstract List<Expression?> parameters {get; set;}
    public abstract int size {get; set;}
    public Command(CodeLocation location) :base(location){}
    public abstract bool CheckParameters(List<CompilingError> errors);
    public bool Check(List<CompilingError> errors)
    {
        for(int i = 0; i < parameters.Count; i++)
        {
            if(parameters[i] == null)
                return false;
            if(!parameters[i].CheckSemantic(errors))
                return false;
        }
        return true;
    }
}