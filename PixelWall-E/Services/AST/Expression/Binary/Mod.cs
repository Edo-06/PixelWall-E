public class Mod : Binary
{
    public override ExpressionType type {get; set;}
    public override object? value {get; set;}

    public Mod(CodeLocation location) : base(location){}

    public override void Evaluate()
    {
        right.Evaluate();
        left.Evaluate();
        
        value = (double)right.value % (double)left.value;
    }

    public override bool CheckSemantic(List<CompilingError> errors)
    {
        bool checkRight = right.CheckSemantic(errors);
        bool checkLeft = left.CheckSemantic(errors);
        if (right.type != ExpressionType.Number || left.type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, ""));
            type = ExpressionType.ErrorType;
            return false;
        }

        type = ExpressionType.Number;
        return checkRight && checkLeft;
    }
}