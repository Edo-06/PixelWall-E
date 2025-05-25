public abstract class ASTNode
{
    public CodeLocation location {get; private set;}
    public abstract T Accept<T>(IVisitor<T> visitor);
    public ASTNode(CodeLocation location)
    {
        this.location = location;
    }
}