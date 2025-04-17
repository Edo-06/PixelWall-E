public abstract class Node
{
    public CodeLocation location {get; set;}
    public abstract bool CheckSemantic(List<CompilingError> errors);
    public Node(CodeLocation location)
    {
        this.location = location;
    }
}