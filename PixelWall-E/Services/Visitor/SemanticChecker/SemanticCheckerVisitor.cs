public class SemanticCheckerVisitor: IVisitor<bool>
{
    public List<CompilingError> errors {get; set;}
    public SemanticCheckerVisitor(List<CompilingError> errors)
    {
        this.errors = errors;
    }
#region Statement
    public bool Visit(ProgramNode program)
    {
        foreach (ASTNode node in program.statements)
        {
            if (!node.Accept(this))
            {
                return false;
            }
        }
        return true;
    }
    public bool Visit(AssignmentNode assignment)
    {
        if (!assignment.expression.Accept(this)) return false;
        assignment.type = assignment.expression.type;

        Scope.variables.Add(assignment.name, assignment.expression);
        Console.WriteLine($"asignando {assignment.name} en SC");
        return true;
    }
    public bool Visit(CommandNode command)
    {
        return ParameterChecker.CheckParameters(command, this, errors, command.location);
    }

    public bool Visit(GoToNode goTo)
    {
        if(!goTo.label.Accept(this)) return false;
        if(!goTo.parameters[0].Accept(this)) return false;
        if(goTo.parameters[0].type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(goTo.location, ErrorCode.Invalid, "Parameter must be a boolean in GoTo statement"));
            return false;
        }
        return true;
    }
    public bool Visit(LabelNode label)
    {
        if(!Scope.labels.ContainsKey(label.name))
        {
            errors.Add(new CompilingError(label.location, ErrorCode.Invalid, $"Label '{label.name}' is not defined"));
            return false;
        }
        return true;
    }
#endregion
#region Expression
    public bool Visit(UnaryOpNode unary)
    {
        unary.operand.Accept(this);
        switch (unary.op)
        {
            case TokenType.Not:
                if (unary.operand.type != ExpressionType.Bool)
                {
                    errors.Add(new CompilingError(unary.location, ErrorCode.Invalid, "Invalid type for logical NOT operation"));
                    return false;
                }
                unary.type = ExpressionType.Bool;
                break;
            case TokenType.Minus:
                if (unary.operand.type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(unary.location, ErrorCode.Invalid, "Invalid type for negation operation"));
                    return false;
                }
                unary.type = ExpressionType.Number;
                break;
            default:
                errors.Add(new CompilingError(unary.location, ErrorCode.Invalid, "Unsupported unary operation"));
                return false;
        }
        return true;
    }
    public bool Visit(BinaryOpNode binary)
    {
        if (!binary.left.Accept(this) || !binary.right.Accept(this)) return false;
        if (binary.left.type != binary.right.type)
        {
            errors.Add(new CompilingError(binary.location, ErrorCode.Invalid, "Types do not match for binary operation"));
            return false;
        }
        switch (binary.op)
        {
            case TokenType.Plus:
            case TokenType.Minus:
            case TokenType.Multiply:
            case TokenType.Divide:
            case TokenType.Modulo:
            case TokenType.Power:
                if(binary.left.type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(binary.location, ErrorCode.Invalid, "Invalid type for arithmetic operation"));
                    return false;
                }
                binary.type = ExpressionType.Number;
                break;
            case TokenType.Equal:
            case TokenType.NotEqual:
            case TokenType.Less:
            case TokenType.Greater:
            case TokenType.LessEqual:
            case TokenType.GreaterEqual:
            case TokenType.And:
            case TokenType.Or:
                binary.type = ExpressionType.Bool;
                break;
            default:
                errors.Add(new CompilingError(binary.location, ErrorCode.Invalid, "Unsupported binary operation"));
                return false;
        }
        return true;
    }
    public bool Visit(LiteralNode literal)
    {
        string value = (string)literal.value;
        string cleaned = value.Replace("\"", "").Trim();
        value = cleaned;
        if(int.TryParse(value , out int number))
        {
            literal.value = number;
            literal.type = ExpressionType.Number;
        }
        else if(bool.TryParse(value , out bool boolean))
        {
            literal.value = boolean;
            literal.type = ExpressionType.Bool;
            /* errors.Add(new CompilingError(literal.location, ErrorCode.Invalid, "Invalid number format"));
            return false; */
        }
        else if(ColorTypes.colors.Contains(value))
        {
            literal.value = value;
            literal.type = ExpressionType.Color;
            /* errors.Add(new CompilingError(literal.location, ErrorCode.Invalid, "Invalid boolean format"));
            return false; */
        }
        else
        {
            errors.Add(new CompilingError(literal.location, ErrorCode.Invalid, "Unsupported literal type"));
            return false;
        }
        return true;
    }
    public bool Visit(VariableNode variable)
    {
        if (!Scope.variables.ContainsKey(variable.name))
        {
            errors.Add(new CompilingError(variable.location, ErrorCode.Invalid, $"Variable '{variable.name}' is not defined"));
            return false;
        }
        variable.type = Scope.variables[variable.name].type; 
        return true;
    }
    public bool Visit(FunctionNode function)
    {
        if(!ParameterChecker.CheckParameters(function, this, errors, function.location)) return false;
        function.type = ParameterChecker.TypeOfFunctionReturn[function.tokenType];
        return true;
    }
#endregion
}