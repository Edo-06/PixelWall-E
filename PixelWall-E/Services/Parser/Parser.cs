public class Parser
{
    private List<Token> tokens;
    private int currentPosition;
    public List<ASTNode> nodes { get; private set; } = new List<ASTNode>();
    public List<Exception> errors { get; private set; } = new List<Exception>();
    public ProgramNode programNode { get; private set; } = new ProgramNode(new CodeLocation());
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        currentPosition = 0;
        // empieza a parsear
        if (tokens[currentPosition].type == TokenType.Spawn)
        {
            nodes.Add(ParseCallable(new CommandNode(tokens[currentPosition].location, tokens[currentPosition].type)));
            ConsumeEOL();
        }
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Spawn"));
        ParseStatements();
    }
    private void ParseStatements()
    {
        while (currentPosition < tokens.Count && tokens[currentPosition].type != TokenType.EndOfFile)
        {
            if (Construct.IsCallableCommand(tokens[currentPosition].type)) //parsea comandos
            {
                if (tokens[currentPosition].type == TokenType.Spawn) //error si es un spawn
                    errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Unexpected, $"Spawn"));
                nodes.Add(ParseCallable(new CommandNode(tokens[currentPosition].location, tokens[currentPosition].type)));
                ConsumeEOL();
            }
            else if (tokens[currentPosition].IsController())
            {
                switch (tokens[currentPosition].type)
                {
                    case TokenType.GoTo:
                        nodes.Add(ParseGoTo(new GoToNode(tokens[currentPosition].location)));
                        ConsumeEOL();
                        break;
                    case TokenType.Identifier:
                        ParseIdentifier();
                        break;
                    case TokenType.EndOfLine:
                        ConsumeEOL();
                        break;
                }
            }
            else
            {
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Unexpected, $"{tokens[currentPosition].lexeme}"));
                ConsumeLine();
            }
        }
        programNode.statements = nodes;
        PipeLineManager.program = programNode;
    }
    #region ParseCommand
    private T ParseCallable<T>(T node) where T : ICallableNode
    {
        int size = Construct.GetElementByToken(node.tokenType).expected.Input.Count;
        Consume(); // Consume the command token
        ParseParameters(node.parameters, size);
        return node;
    }
    private T ParseGoTo<T>(T node) where T : GoToNode
    {
        int size = node.excpectedParametersCount;
        Consume(); //GoTo
        if (tokens[currentPosition].type == TokenType.LeftBracket)
            Consume(); //LeftBracket
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "'['"));
        if (tokens[currentPosition].type == TokenType.Identifier)
        {
            LabelNode label = new LabelNode(tokens[currentPosition].location, tokens[currentPosition].lexeme, nodes.Count);
            node.label = label;
            Consume();// identifier
        }
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "label"));
        if (tokens[currentPosition].type == TokenType.RightBracket)
            Consume(); //RightBracket
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "']'"));
        ParseParameters(node.parameters, size);
        return node;
    }
    private void ParseParameters(List<ExpressionNode> parameters, int expectedSize)
    {
        if (tokens[currentPosition].type == TokenType.LeftParen)
            Consume(); // Skip the '(' token
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "'('"));
        for (int i = 0; i < expectedSize; i++)
        {
            if (i >= 1)
            {
                if (tokens[currentPosition].type == TokenType.Comma)
                    Consume(); // Skip the ',' token
                else
                    errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, $"','"));
            }
            ExpressionNode newExpression = ParseExpression();
            if (newExpression == null)
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Invalid, "expression"));
            else
                parameters.Add(newExpression);
        }
        if (tokens[currentPosition].type == TokenType.RightParen)
            Consume(); //RightBracket
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "')'"));
    }
    #endregion
    #region ParseExpression
    private ExpressionNode ParseExpression()
    {
        return ParseOr(); //menor precedencia
    }
    private ExpressionNode ParseOr()
    {
        ExpressionNode expression = ParseAnd(); //lado izquierdo como AND
        Token token = tokens[currentPosition];
        while (token.type == TokenType.Or)
        {
            Consume();
            ExpressionNode right = ParseAnd(); //parsea la siguiente precedencia: AND
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseAnd()
    {
        ExpressionNode expression = ParseComparision(); //lado izquierdo como COMPARISION
        Token token = tokens[currentPosition];
        while (token.type == TokenType.And)
        {
            Consume();
            ExpressionNode right = ParseComparision();
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseComparision()
    {
        ExpressionNode expression = ParseAddSub(); //lado izquierdo como ADD/SUB
        Token token = tokens[currentPosition];
        while (token.type == TokenType.Greater || token.type == TokenType.GreaterEqual
        || token.type == TokenType.Less || token.type == TokenType.LessEqual || token.type == TokenType.Equal)
        {
            Consume();
            ExpressionNode right = ParseAddSub();
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseAddSub()
    {
        ExpressionNode expression = ParseMulDivMod(); //lado izquierdo como MUL/DIV/MOD
        Token token = tokens[currentPosition];
        while (token.type == TokenType.Plus || token.type == TokenType.Minus)
        {
            Consume();
            ExpressionNode right = ParseMulDivMod();
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseMulDivMod()
    {
        ExpressionNode expression = ParsePower(); //lado izquierdo como POWER
        Token token = tokens[currentPosition];
        while (token.type == TokenType.Multiply || token.type == TokenType.Divide ||
        tokens[currentPosition].type == TokenType.Modulo)
        {
            Consume();
            ExpressionNode right = ParsePower();
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParsePower()
    {
        ExpressionNode expression = ParseUnary(); //lado izquierdo como expresion unaria
        Token token = tokens[currentPosition];
        while (token.type == TokenType.Power)
        {
            Consume();
            ExpressionNode right = ParseUnary();
            expression = new BinaryOpNode(token.location, expression, token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseUnary()
    {
        Token token = tokens[currentPosition];
        if (token.type == TokenType.Minus || token.type == TokenType.Not)
        {
            Consume();
            ExpressionNode operand = ParseUnary();
            ExpressionNode rt = new UnaryOpNode(token.location, token.type, operand);
            token = tokens[currentPosition];
            return rt;
        }
        else //no hay operador unario
        {
            return ParseAtom(); //parsea literales o variables
        }
    }
    private ExpressionNode ParseAtom()
    {
        if (currentPosition > tokens.Count - 1)
            return null!;
        Token token = tokens[currentPosition];
        switch (tokens[currentPosition].type)
        {
            case TokenType.Number:
                Consume();
                Console.WriteLine(tokens[currentPosition].lexeme + tokens[currentPosition].type);
                return new LiteralNode(token.location, token.lexeme);
            case TokenType.Identifier:
                Consume();
                Console.WriteLine($"{token.lexeme} ...............");
                return new VariableNode(token.location, token.lexeme);
            case TokenType.ColorString:
                Consume();
                return new LiteralNode(token.location, token.lexeme);
            case TokenType.Bool:
                Consume();
                return new LiteralNode(token.location, token.lexeme);
            case TokenType.LeftParen:
                Consume();
                ExpressionNode expression = ParseExpression();
                Consume(); //RightParen
                return expression;
            default:
                if (Construct.IsCallableExpression(tokens[currentPosition].type))
                {
                    Console.WriteLine("default function");
                    return ParseCallable(new FunctionNode(tokens[currentPosition].location, tokens[currentPosition].type));
                }
                return null!;
        }
    }
    #endregion
    #region ParseIdentifier
    private void ParseIdentifier()
    {
        Token token = tokens[currentPosition];
        Console.WriteLine(token.type);
        //labels o asignaciones
        Console.WriteLine(token.location.column);
        if (token.location.column == 0)
        {
            //label
            Consume();
            if (tokens[currentPosition].type == TokenType.EndOfLine || tokens[currentPosition].type == TokenType.EndOfFile)
            {
                Console.WriteLine("here");
                if (Scope.labels.ContainsKey(token.lexeme))
                    errors.Add(new CompilingError(token.location, ErrorCode.Invalid, $"label {token.lexeme} already exists"));
                else
                    Scope.labels.Add(token.lexeme, nodes.Count);
                return;
            }
            //asignacion
            if (tokens[currentPosition].type == TokenType.AssignArrow)
            {
                AssignmentNode assignment = new AssignmentNode(token.location, token.lexeme);
                Consume();
                ExpressionNode expression = ParseExpression();
                if (tokens[currentPosition].type == TokenType.EndOfLine || tokens[currentPosition].type == TokenType.EndOfFile) ConsumeEOL();
                else
                    errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "end of line"));
                if (expression != null)
                {
                    assignment.expression = expression;
                    nodes.Add(assignment);
                }
                else
                    errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Invalid, "expression"));
                return;
            }
            ConsumeLine();
            errors.Add(new CompilingError(token.location, ErrorCode.Invalid, "identifier usage"));
        }
    }
    #endregion
    private void ConsumeEOL()
    {
        if (currentPosition < tokens.Count - 1)
        {
            if (tokens[currentPosition].type != TokenType.EndOfLine)
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "end of line"));
            while (currentPosition < tokens.Count - 1 && tokens[currentPosition].type == TokenType.EndOfLine)
            {
                Consume(); //Skip EOL tokens
            }
        }
    }
    private void ConsumeLine()
    {
        while (currentPosition < tokens.Count && tokens[currentPosition].type != TokenType.EndOfLine && tokens[currentPosition].type != TokenType.EndOfFile)
        {
            Consume();
        }
        if (currentPosition < tokens.Count && tokens[currentPosition].type == TokenType.EndOfLine)
            Consume(); // Skip the EndOfLine token
    }
    private void Consume()
    {
        if (currentPosition + 1 < tokens.Count) currentPosition++;
        return;
    }
}