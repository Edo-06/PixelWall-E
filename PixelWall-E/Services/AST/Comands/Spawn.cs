public class Spawn : Node
{
    public object? x {get; set;}
    public object? y {get; set;}

    public Spawn(CodeLocation location, object x, object y): base(location)
    {
        this.x = x;
        this.y = y; 
    }

    public override bool CheckSemantic(){return true;}
}