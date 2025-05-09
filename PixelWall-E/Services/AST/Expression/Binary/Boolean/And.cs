public class And: Binary
{
    public override ExpressionType type {get; set;}
    public override object? value {get; set;}
    public And(CodeLocation location, Expression? left, Expression? right) : base(location, left, right){}

    public override Task Evaluate()
    {
        if(right == null || left == null)
            return Task.CompletedTask;
            right.Evaluate();
            left.Evaluate();
        if(right.value == null || left.value == null)
            return Task.CompletedTask;
            this.value = (bool)right.value && (bool)left.value;
            return Task.CompletedTask;
        
    }
    public override bool CheckSemantic(List<CompilingError> errors)
    {
        if (right == null || left == null)
        {
            type = ExpressionType.ErrorType;
            return false;
        }
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