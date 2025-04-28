public class Equal: Binary
{
    public override ExpressionType type {get; set;}
    public override object? value {get; set;}
    public Equal(CodeLocation location, Expression left, Expression right) : base(location, left, right){}

    public override void Evaluate()
    {
        if(right != null && left != null)
        {   
            right.Evaluate();
            left.Evaluate();
            this.value = (bool)right.value == (bool)left.value;
        }
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        bool checkRight = right.CheckSemantic(errors);
        bool checkLeft = left.CheckSemantic(errors);
        if (right.type != ExpressionType.Bool || left.type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, ""));
            type = ExpressionType.ErrorType;
            return false;
        }

        type = ExpressionType.Bool;
        return checkLeft && checkRight;
    }
}