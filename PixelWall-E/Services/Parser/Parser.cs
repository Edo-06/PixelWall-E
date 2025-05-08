public class Parser
{
    private List<Token> tokens;
    private int currentPosition;
    public List<Node> nodes {get; private set;} = new List<Node>();
    public List<CompilingError> errors {get; private set;} = new List<CompilingError>();
    public ProgramNode programNode {get; private set;} = new ProgramNode(new CodeLocation());
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        currentPosition = 0;
        ParseStatements();
    }
    private void ParseStatements()
    {
        while (currentPosition < tokens.Count - 1)
        {
            switch (tokens[currentPosition].type)
            {
                //Command
                case TokenType.Spawn:
                    nodes.Add(ParseCommand(new Spawn(tokens[currentPosition].location)));
                    break;
                case TokenType.Color:
                    nodes.Add(ParseCommand(new Color(tokens[currentPosition].location)));
                    break;
                case TokenType.Fill:
                    nodes.Add(ParseCommand(new Fill(tokens[currentPosition].location)));
                    break;
                case TokenType.Size:
                    nodes.Add(ParseCommand(new Size(tokens[currentPosition].location)));
                    break;
                case TokenType.DrawCircle:
                    nodes.Add(ParseCommand(new DrawCircle(tokens[currentPosition].location)));
                    break;
                case TokenType.DrawLine:
                    nodes.Add(ParseCommand(new DrawLine(tokens[currentPosition].location)));
                    break;
                case TokenType.DrawRectangle:
                    nodes.Add(ParseCommand(new DrawRectangle(tokens[currentPosition].location)));
                    break;
                
                //FUnction
                case TokenType.GetActualX:
                    nodes.Add(ParseCommand(new GetActualX(tokens[currentPosition].location)));
                    break;
                case TokenType.GetActualY:
                    nodes.Add(ParseCommand(new GetActualY(tokens[currentPosition].location)));
                    break;
                case TokenType.GetCanvasSize:
                    nodes.Add(ParseCommand(new GetCanvasSize(tokens[currentPosition].location)));
                    break;
                case TokenType.GetColorCount:
                    nodes.Add(ParseCommand(new GetColorCount(tokens[currentPosition].location)));
                    break;
                case TokenType.IsBrushColor:
                    nodes.Add(ParseCommand(new IsBrushColor(tokens[currentPosition].location)));
                    break;
                case TokenType.IsBrushSize:
                    nodes.Add(ParseCommand(new IsBrushSize(tokens[currentPosition].location)));
                    break;
                case TokenType.IsCanvasColor:
                    nodes.Add(ParseCommand(new IsColor(tokens[currentPosition].location)));
                    break;
                //Identifier
                case TokenType.Identifier:
                    ParseIdentifier();
                    break;
                //GoTo
                case TokenType.GoTo:
                    nodes.Add(ParseGoTo(new GoTo(tokens[currentPosition].location)));
                    break;
                default:
                    ConsumeWithEOL();
                    break;
            }
        }
        programNode.nodes = nodes;
        PipeLineManager.nodes = nodes;
    }
#region ParseCommand
    private T ParseCommand<T>(T node) where T : Command
    {
        Consume(); // Consume the command token
        ParseParameters(node.parameters, node.size);
        return node;
    }
    private T ParseGoTo<T>(T node) where T: GoTo
    {
        Consume(); 
        if(tokens[currentPosition].type == TokenType.LeftBracket)
            Consume();
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected a '['"));
        if(tokens[currentPosition].type == TokenType.Identifier)
        {
            Label label = new Label(tokens[currentPosition].lexeme, tokens[currentPosition].location);
            node.label = label;
            Consume();
        }
        else
            errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Expected, "Expected a label"));
        ParseParameters(node.parameters, node.size);
        return node;
    }
    private void ParseParameters(List<Expression?> parameters, int expectedSize)
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
            Expression? newExpression = ParseExpression();
            if (newExpression == null)
            {
                errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Invalid,"Invalid expression"));
            }
            parameters.Add(newExpression);
        }
        if(tokens[currentPosition].type != TokenType.RightParen)
            errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a ')'"));
        ConsumeWithEOL(); // Skip the ')' token
    }
    //
#endregion
#region ParseExpression
    private Expression? ParseExpression()
    {
        return ParseOr();
    }
    private Expression? ParseOr()
    {
        Expression? expression = ParseAnd();
        while(tokens[currentPosition].type == TokenType.Or)
        {
            Consume();
            Expression? right = ParseAnd();
            expression = new Or(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParseAnd()
    {
        Expression? expression = ParseComparision();
        while(tokens[currentPosition].type == TokenType.And)
        {
            Consume();
            Expression? right = ParseComparision();
            expression = new And(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParseComparision()
    {
        Expression? expression = ParseAddSub();
        while(tokens[currentPosition].type == TokenType.Greater || tokens[currentPosition].type == TokenType.GreaterEqual 
        ||tokens[currentPosition].type == TokenType.Less || tokens[currentPosition].type == TokenType.LessEqual || tokens[currentPosition].type == TokenType.Equal)
        {
            Consume();
            Expression? right = ParseAddSub();
            expression = new Equal(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParseAddSub()
    {
        Expression? expression = ParseMulDivMod();
        while(tokens[currentPosition].type == TokenType.Plus || tokens[currentPosition].type == TokenType.Minus)
        {
            Consume();
            Expression? right = ParseMulDivMod();
            expression = new Add(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParseMulDivMod()
    {
        Expression? expression = ParsePower();
        while(tokens[currentPosition].type == TokenType.Multiply || tokens[currentPosition].type == TokenType.Divide ||
        tokens[currentPosition].type == TokenType.Modulo)
        {
            Consume();
            Expression? right = ParsePower();
            expression = new Add(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParsePower()
    {
        Expression? expression = ParseAtom();
        while(tokens[currentPosition].type == TokenType.Power)
        {
            Consume();
            Expression? right = ParseAtom();
            expression = new Add(tokens[currentPosition - 1].location, expression, right);
        }
        return expression;
    }
    private Expression? ParseAtom()
    {
        if(currentPosition >= tokens.Count - 1)
            return null;
        Token actual = tokens[currentPosition];
        switch(tokens[currentPosition].type)
        {
            case TokenType.Number:
                Consume();
                return new Number(actual.lexeme, actual.location);
            case TokenType.Identifier:
                Consume();
                return new Variable(actual.lexeme, actual.location);
            case TokenType.ColorString:
                Consume();
                return new ColorString(actual.location, actual.lexeme);
            case TokenType.LeftParen:
                Consume();
                Expression? expression = ParseExpression();
                Consume(); //RightParen
                return expression;
            default:
                return null;
        }
    }
#endregion
#region ParseIdentifier
    private void ParseIdentifier()
    {
        string name = tokens[currentPosition].lexeme;
        CodeLocation location = tokens[currentPosition].location;
        Consume();
        if(tokens[currentPosition].type == TokenType.EndOfLine)
        {
            Scope.labels.Add(name, location.line);
        }
        else if(tokens[currentPosition].type == TokenType.AssignArrow)
        {
            Assignament assignament = new Assignament(name, location);
            Consume();
            Expression? expression = ParseExpression();
            if(expression == null)
                errors.Add(new CompilingError(tokens[currentPosition].location, ErrorCode.Invalid,"Invalid expression"));
            assignament.expression = expression;
            nodes.Add(assignament);
        }
        errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a '<-'"));
        ConsumeWithEOL();
    }
#endregion
    private void ConsumeWithEOL()
    {
        if(currentPosition < tokens.Count - 1)
        {
            Consume(); //Skip current token != EOL
            while(currentPosition < tokens.Count - 1 && tokens[currentPosition].type == TokenType.EndOfLine)
            {
                Consume(); //Skip EOL tokens
            }
        }
    }
    private void Consume()
    {
        currentPosition++;
    }
}