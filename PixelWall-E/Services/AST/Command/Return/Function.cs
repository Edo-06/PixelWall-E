public abstract class Function: Command
{
    public abstract Expression? rtn {get; set;}
    public Function(CodeLocation location):base(location){}
    public abstract void Evaluate();
}