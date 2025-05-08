public class Add: Binary
{
    public override ExpressionType type {get; set;}
    public override object? value {get; set;}
    public Add(CodeLocation location, Expression? left, Expression? right) : base(location, left, right){}

    public override void Evaluate()
    {
        if(right != null && left != null)
        {   
            right.Evaluate();
            left.Evaluate();
            this.value = (int)right.value + (int)left.value;
        }
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
        return checkLeft && checkRight;
    }
}