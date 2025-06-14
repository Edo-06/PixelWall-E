using System.Runtime.CompilerServices;

public class Executor : IVisitor<Task>
{
    public List<CompilingError> errors {get; set;} = [];
    public Executor(){}
    private int currentStatement = 0;
    public async Task Visit(ProgramNode program)
    {
        while(currentStatement < program.statements.Count && PipeLineManager.isRunning)
        {
            await program.statements[currentStatement].Accept(this);
            currentStatement++;
        }
            return;
    }

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
                throw new NotImplementedException($"Unary operator {unary.op} is not a unary operator.");
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
                if(Operators<int>.AritmeticOperator.ContainsKey(binary.op)) 
                    binary.value = Operators<int>.AritmeticOperator[binary.op].Invoke((int)binary.left.value, (int)binary.right.value);
                else if(Operators<int>.ComparisionOperator.ContainsKey(binary.op))
                    binary.value = Operators<int>.ComparisionOperator[binary.op].Invoke((int)binary.left.value, (int)binary.right.value);
                break;
            case ExpressionType.Bool:
                if(Operators<bool>.BooleanOperator.ContainsKey(binary.op))
                    binary.value = Operators<bool>.BooleanOperator[binary.op].Invoke((bool)binary.left.value, (bool)binary.right.value);
                else if(Operators<bool>.ComparisionOperator.ContainsKey(binary.op))
                    binary.value = Operators<bool>.ComparisionOperator[binary.op].Invoke((bool)binary.left.value, (bool)binary.right.value);
                break;
            default:
                throw new NotImplementedException($"Binary operation for type {binary.left.type} is not implemented.");
        }
        return Task.CompletedTask;
    }

    public Task Visit(LiteralNode literal)
    {
        return Task.CompletedTask;
    }

    public Task Visit(FunctionNode function)
    {
        foreach(ExpressionNode parameter in function.parameters)
        {
            parameter.Accept(this);
        }
        function.value = HandlerFunction.Execute(function);
        return Task.CompletedTask;
    }

    public Task Visit(VariableNode variable)
    {
        if(!Scope.variables.ContainsKey(variable.name))
            errors.Add(new CompilingError(variable.location, ErrorCode.Invalid, $"Variable '{variable.name}' is not defined."));
        else
            variable.value = Scope.variables[variable.name].value;
            Console.WriteLine($"Variable {variable.name} = {variable.value}");
        return Task.CompletedTask;
    }

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

    public async Task Visit(GoToNode goTo)
    {
        await goTo.parameters[0].Accept(this);
        Console.WriteLine($"GoToNode: {goTo.label.name} with value {goTo.parameters[0].value}");
        if((bool)goTo.parameters[0].value)
        {
            Console.WriteLine($"Jumping to label {goTo.label.name}");
            Console.WriteLine($"{Scope.labels[goTo.label.name]} to {goTo.label.breakP}");
            currentStatement = Scope.labels[goTo.label.name] - 1;
            Console.WriteLine("ya");
        }
    }

    public Task Visit(LabelNode label)
    {
        return Task.CompletedTask;
    }
}