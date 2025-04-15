public class Div : Binary
{
    public override ExpressionType type {get; set;}
    public override object? value {get; set;}

    public Div(CodeLocation location) : base(location){}

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        
        value = (double)Right.value / (double)Left.value;
    }

    public override bool CheckSemantic()
    {
        bool right = Right.CheckSemantic();
        bool left = Left.CheckSemantic();
        if (Right.type != ExpressionType.Number || Left.type != ExpressionType.Number)
        {
            type = ExpressionType.ErrorType;
            return false;
        }

        type = ExpressionType.Number;
        return right && left;
    }

}