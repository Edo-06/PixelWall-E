public class Parser
{
    private List<Token> tokens;
    private int currentPosition;
    public List<ASTNode> nodes {get; private set;} = new List<ASTNode>();
    public List<CompilingError> errors {get; private set;} = new List<CompilingError>();
    public ProgramNode programNode {get; private set;} = new ProgramNode(new CodeLocation());
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        currentPosition = 0;
        ParseStatements();
    }
        private void ConsumeWithEOL()
    {
        if(currentPosition < tokens.Count - 1)
        {
            Consume(); //Skip current token != EOL
            if(tokens[currentPosition].type != TokenType.EndOfLine)
            {
                Console.WriteLine("here");
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected an EndOfLine token"));
            }
            while(currentPosition < tokens.Count - 1 && tokens[currentPosition].type == TokenType.EndOfLine)
            {
                Consume(); //Skip EOL tokens
            }
        }
    }
    private void Consume()
    {
        if(currentPosition + 1 < tokens.Count)
            currentPosition++;
        return;
    }
    private void ParseStatements()
    {
        while (currentPosition < tokens.Count - 1)
        {
            if(tokens[currentPosition].IsCallable())
            {
                nodes.Add(ParseCallable(new CommandNode(tokens[currentPosition].location, tokens[currentPosition].type)));
            }
            else if(tokens[currentPosition].IsController())
            {
                switch (tokens[currentPosition].type)
                {
                    case TokenType.GoTo:
                        nodes.Add(ParseGoTo(new GoToNode(tokens[currentPosition].location)));
                        break;
                    case TokenType.Identifier:
                        ParseIdentifier();
                        break;
                    case TokenType.EndOfLine:
                        ConsumeWithEOL();
                        break;
                }
            }
            else
            {
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Invalid, "Invalid token"));
                Consume();
            }
        }
        programNode.statements = nodes;
        PipeLineManager.program = programNode;
    }
#region ParseCommand
    private T ParseCallable<T>(T node) where T : ICallableNode
    {
        int size = tokens[currentPosition].expectedParameterCounts[tokens[currentPosition].type];
        Consume(); // Consume the command token
        ParseParameters(node.parameters, size);
        return node;
    }
    private T ParseGoTo<T>(T node) where T: GoToNode
    {
        int size = tokens[currentPosition].expectedParameterCounts[tokens[currentPosition].type];
        Consume(); 
        if(tokens[currentPosition].type == TokenType.LeftBracket)
            Consume();
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected a '['"));
        if(tokens[currentPosition].type == TokenType.Identifier)
        {
            LabelNode label = new LabelNode(tokens[currentPosition].location, tokens[currentPosition].lexeme, nodes.Count - 1);
            node.label = label;
            Consume();
        }
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected a label"));
        if(tokens[currentPosition].type == TokenType.RightBracket)
            Consume();
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected a ']'"));
        ParseParameters(node.parameters, size);
        return node;
    }
    private void ParseParameters(List<ExpressionNode> parameters, int expectedSize)
    {
        if(tokens[currentPosition].type == TokenType.LeftParen)
            Consume(); // Skip the '(' token
        else
            errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a '('"));
        for(int i = 0; i < expectedSize; i++)
        {
            if(i >= 1)
            {
                if(tokens[currentPosition].type == TokenType.Comma)
                    Consume(); // Skip the ',' token
                else
                    errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,$"Expected a ','"));
            }
            ExpressionNode newExpression = ParseExpression();
            if (newExpression == null)
                errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Invalid,"Invalid expression"));
            else
                parameters.Add(newExpression);
        }
        if(tokens[currentPosition].type != TokenType.RightParen)
            errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a ')'"));
        ConsumeWithEOL(); // Skip the ')' token
    }
    //
#endregion
#region ParseExpression
    private ExpressionNode ParseExpression()
    {
        return ParseOr();
    }
    private ExpressionNode ParseOr()
    {
        ExpressionNode expression = ParseAnd();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.Or)
        {
            Consume();
            ExpressionNode right = ParseAnd();
            expression = new BinaryOpNode(token.location, expression ,token.type, right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseAnd()
    {
        ExpressionNode expression = ParseComparision();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.And)
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
        ExpressionNode expression = ParseAddSub();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.Greater || token.type == TokenType.GreaterEqual 
        ||token.type == TokenType.Less || token.type == TokenType.LessEqual || token.type == TokenType.Equal)
        {
            Consume();
            ExpressionNode right = ParseAddSub();
            expression = new BinaryOpNode(token.location, expression,token.type , right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseAddSub()
    {
        ExpressionNode expression = ParseMulDivMod();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.Plus || token.type == TokenType.Minus)
        {
            Consume();
            ExpressionNode right = ParseMulDivMod();
            expression = new BinaryOpNode(token.location, expression, token.type,right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParseMulDivMod()
    {
        ExpressionNode expression = ParsePower();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.Multiply || token.type == TokenType.Divide ||
        tokens[currentPosition].type == TokenType.Modulo|| token.type == TokenType.Power)
        {
            Consume();
            ExpressionNode right = ParsePower();
            expression = new BinaryOpNode(token.location, expression, token.type,right);
            token = tokens[currentPosition];
        }
        return expression;
    }
    private ExpressionNode ParsePower()
    {
        ExpressionNode expression = ParseUnary();
        Token token = tokens[currentPosition];
        while(token.type == TokenType.Power)
        {
            Consume();
            ExpressionNode right = ParseUnary();
            expression = new BinaryOpNode(token.location, expression, token.type,right);
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
        else 
        {
            return ParseAtom();
        }
    }
    private ExpressionNode ParseAtom()
    {
        if(currentPosition > tokens.Count - 1)
            return null!;
        Token token = tokens[currentPosition];
        Console.WriteLine(token.lexeme);
        switch(tokens[currentPosition].type)
        {
            case TokenType.Number:
                Consume();
                Console.WriteLine(tokens[currentPosition].lexeme + tokens[currentPosition].type);
                return new LiteralNode(token.location, token.lexeme);
            case TokenType.Identifier:
                Consume();
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
                if(tokens[currentPosition].IsCallableExpression())
                {
                    nodes.Add(ParseCallable(new CommandNode(tokens[currentPosition].location, tokens[currentPosition].type)));
                }
                errors.Add(new CompilingError(token.location, ErrorCode.Invalid, "Invalid expression"));
                return null!;
        }
    }
#endregion
#region ParseIdentifier
    private void ParseIdentifier()
    {
        Token token = tokens[currentPosition];
        Console.WriteLine(token.type);
        Consume();
        Console.WriteLine(tokens[currentPosition].type);
        if(tokens[currentPosition].type == TokenType.EndOfLine)
        {
            if(Scope.labels.ContainsKey(token.lexeme))
            {
                Scope.labels[token.lexeme] = nodes.Count;
                Consume();
                return;
            }
            Scope.labels.Add(token.lexeme, nodes.Count);
            Consume();
            return;
        }
        else if(tokens[currentPosition].type == TokenType.AssignArrow)
        {
            AssignmentNode assignment = new AssignmentNode(token.location, token.lexeme);
            Consume();
            ExpressionNode expression = ParseExpression();
            if(expression != null)
            {
                assignment.expression = expression;
                nodes.Add(assignment);
            }
            else
            {
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Invalid,"Invalid expression"));
            }
            Consume();
            return;
        }
        errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a '<-'"));
        ConsumeWithEOL();
    }
#endregion
}