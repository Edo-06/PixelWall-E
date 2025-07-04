public class Executor : IVisitor<Task>
{
    public List<Exception> errors { get; set; } = [];
    public Executor() { }
    private int currentStatement = 0;
    public async Task Visit(ProgramNode program)
    {
        while (currentStatement < program.statements.Count && PipeLineManager.isRunning)
        {
            await program.statements[currentStatement].Accept(this);
            currentStatement++;
        }
        return;
    }
#region Expression
    public Task Visit(UnaryOpNode unary)
    {
        unary.operand.Accept(this);
        switch (unary.op)
        {
            case TokenType.Minus:
                unary.value = -(int)unary.operand.value;
                break;
            case TokenType.Not:
                unary.value = !(bool)unary.operand.value;
                break;
            default:
                throw new RuntimeError(unary.location, RuntimeErrorCode.NotImplemented, $"unary operator {unary.op} is not a unary operator (semantic checker missed this).");
        }
        return Task.CompletedTask;
    }

    public Task Visit(BinaryOpNode binary)
    {
        binary.left.Accept(this);
        binary.right.Accept(this);
        switch (binary.left.type)
        {
            case ExpressionType.Number:
                if (Operators<int>.AritmeticOperator.ContainsKey(binary.op))
                {
                    try
                    {
                        binary.value = Operators<int>.AritmeticOperator[binary.op].Invoke((int)binary.left.value, (int)binary.right.value);
                    }
                    catch (DivideByZeroException ex)
                    {
                        throw new RuntimeError(binary.location, RuntimeErrorCode.DivisionByZero, ex.Message);
                    }
                }
                else if (Operators<int>.ComparisionOperator.ContainsKey(binary.op))
                    binary.value = Operators<int>.ComparisionOperator[binary.op].Invoke((int)binary.left.value, (int)binary.right.value);
                break;
            case ExpressionType.Bool:
                if (Operators<bool>.BooleanOperator.ContainsKey(binary.op))
                    binary.value = Operators<bool>.BooleanOperator[binary.op].Invoke((bool)binary.left.value, (bool)binary.right.value);
                else if (Operators<bool>.ComparisionOperator.ContainsKey(binary.op))
                    binary.value = Operators<bool>.ComparisionOperator[binary.op].Invoke((bool)binary.left.value, (bool)binary.right.value);
                break;
            default:
                throw new RuntimeError(binary.location, RuntimeErrorCode.InvalidOperation, $"binary operation not implemented for type '{binary.left.type}' (semantic checker missed this).");
        }
        return Task.CompletedTask;
    }

    public Task Visit(LiteralNode literal)
    {
        return Task.CompletedTask;
    }

    public Task Visit(FunctionNode function)
    {
        foreach (ExpressionNode parameter in function.parameters)
        {
            parameter.Accept(this);
        }
        function.value = HandlerFunction.Execute(function);
        return Task.CompletedTask;
    }

    public Task Visit(VariableNode variable)
    {
        if (!Scope.variables.ContainsKey(variable.name))
            errors.Add(new CompilingError(variable.location, ErrorCode.Invalid, $"variable '{variable.name}' is not defined."));
        else
            variable.value = Scope.variables[variable.name].value;
        Console.WriteLine($"Variable {variable.name} = {variable.value}");
        return Task.CompletedTask;
    }
#endregion
#region Statement
    public Task Visit(AssignmentNode assignment)
    {
        assignment.expression.Accept(this);
        Scope.variables[assignment.name] = assignment.expression;
        Console.WriteLine($"asignando {assignment.name} = {assignment.expression.value}");
        return Task.CompletedTask;
    }

    public async Task Visit(CommandNode command)
    {
        foreach (ExpressionNode parameter in command.parameters)
        {
            await parameter.Accept(this);
        }
        await HandlerCommand.Execute(command);
    }
    public Task Visit(LabelNode label)
    {
        return Task.CompletedTask;
    }
#endregion
    public async Task Visit(GoToNode goTo)
    {
        await goTo.parameters[0].Accept(this);
        Console.WriteLine($"GoToNode: {goTo.label.name} with value {goTo.parameters[0].value}");
        if ((bool)goTo.parameters[0].value)
        {
            CheckLoop(goTo.location, goTo);
            Console.WriteLine($"Jumping to label {goTo.label.name}");
            Console.WriteLine($"{Scope.labels[goTo.label.name]} to {goTo.label.breakP}");
            currentStatement = Scope.labels[goTo.label.name] - 1;
            Console.WriteLine("ya");
        }
    }
    #region Loop Detection
    private Dictionary<CodeLocation, int> gotoVisitCounts = new Dictionary<CodeLocation, int>();
    private const int GOTO_THRESHOLD = 1000;

    private void CheckLoop(CodeLocation counter, GoToNode goTo)
    {
        if (gotoVisitCounts.ContainsKey(counter))
            gotoVisitCounts[counter]++;
        else
            gotoVisitCounts[counter] = 1;

        if (gotoVisitCounts[counter] > GOTO_THRESHOLD)
        {
            throw new RuntimeError(goTo.location, RuntimeErrorCode.InfiniteLoopDetected,
                $"possible infinite loop detected: label '{goTo.label.name}' visited too many times ({gotoVisitCounts[counter]} times).");
        }
    }
    #endregion
}