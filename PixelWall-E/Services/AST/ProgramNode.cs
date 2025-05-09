public class ProgramNode: Node
{
    public List<Node> nodes {get; set;}
    public List<CompilingError> errors {get; set;}

    public ProgramNode(CodeLocation location):base(location)
    {
        errors = new List<CompilingError>();
        nodes = new List<Node>();
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        foreach(Node node in nodes)
        {
            if(!node.CheckSemantic(errors))
            {
                return false;
            }
        }
        return true;
    }
    public override Task Evaluate()
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            if(!nodes[i].CheckSemantic(errors) || errors.Count > 0)
                return Task.CompletedTask;
            Console.WriteLine($"Evaluating node {i} of type {nodes[i].GetType()}");
            nodes[i].Evaluate();
        }
        return Task.CompletedTask;
    }
}