public class Executor : IVisitor<Task>
{
    public List<CompilingError> errors {get; set;} = [];
    public Executor(){}
    public async Task Visit(ProgramNode program)
    {
        foreach (var statement in program.statements)
        {
            await statement.Accept(this);
        }
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
                try
                {
                    binary.value = int.Parse($"{binary.left.value} {Operators.OperatorSymbols[binary.op]} {binary.right.value}");
                }
                catch(DivideByZeroException)
                {
                    errors.Add(new CompilingError(binary.location, ErrorCode.Invalid, "Division by zero is not allowed."));
                }
                binary.value = int.Parse($"{binary.left.value} {Operators.OperatorSymbols[binary.op]} {binary.right.value}");
                break;
            case ExpressionType.Bool:
                binary.value = bool.Parse($"{binary.left.value} {Operators.OperatorSymbols[binary.op]} {binary.right.value}");
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
        foreach (ExpressionNode parameter in function.parameters)
        {
            parameter.Accept(this);
        }
        switch(function.tokenType)
        {
            case TokenType.GetActualX:  
                function.value = PipeLineManager.currentPixel.x;
                break;
            case TokenType.GetActualY:
                function.value = PipeLineManager.currentPixel.y;
                break;
            case TokenType.GetCanvasSize:
                function.value = PipeLineManager.GetCanvasSize();
                break;
            case TokenType.GetColorCount:
                //function.value = PipeLineManager.GetColorCount();
                break;
            case TokenType.IsBrushColor:
                function.value = PipeLineManager.brushColor;
                break;
            case TokenType.IsBrushSize:
                //function.value = PipeLineManager.
                break;
            case TokenType.IsCanvasColor:
                //function.value = PipeLineManager.IsCanvasColor();
                break;
            default:
                throw new NotImplementedException($"Function {function.tokenType} is not implemented.");
        }
        return Task.CompletedTask;
    }

    public Task Visit(VariableNode variable)
    {
        if(!Scope.variables.ContainsKey(variable.name))
            errors.Add(new CompilingError(variable.location, ErrorCode.Invalid, $"Variable '{variable.name}' is not defined."));
        else
            variable.value = Scope.variables[variable.name].value;
        return Task.CompletedTask;
    }

    public Task Visit(AssignmentNode assignment)
    {
        assignment.expression.Accept(this);
        Scope.variables.Add(assignment.name, assignment.expression);
        return Task.CompletedTask;
    }

    public async Task Visit(CommandNode command)
    {
        foreach (ExpressionNode parameter in command.parameters)
        {
            await parameter.Accept(this);
        }
        await Handler.Execute(command);
    }

    public Task Visit(GoToNode goTo)
    {
        return Task.CompletedTask;
    }

    public Task Visit(LabelNode label)
    {
        return Task.CompletedTask;
    }
}