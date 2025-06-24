public static class ParameterChecker
{
    public static bool CheckParameters<T>(T node, IVisitor<bool> visitor, List<Exception> errors, CodeLocation location) where T: ICallableNode
    {
        List<ExpressionType> toCheck = [];
        toCheck = Construct.GetElementByToken(node.tokenType).expected.Input;
        for(int i = 0; i < toCheck.Count; i++)
        {
            if(!node.parameters[i].Accept(visitor)) return false;
            if(node.parameters[i].type != toCheck[i])
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, $"parameter {i + 1} must be of type {toCheck[i]} in {node.tokenType}"));
                return false;
            }
        }
        return true;
    }
}