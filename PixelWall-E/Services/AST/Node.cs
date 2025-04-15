public abstract class Node
    {
        public CodeLocation location {get; set;}
        public abstract bool CheckSemantic();
        public Node(CodeLocation location)
        {
            this.location = location;
        }
    }