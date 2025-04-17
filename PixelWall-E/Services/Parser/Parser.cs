using System.Drawing;

public class Parser
{
    private List<Token> tokens;
    private int currentPosition;

    Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }
#region ParseCommand
    private Spawn ParseSpawn(List<CompilingError> errors)
    {
        Spawn spawn = new Spawn(tokens[currentPosition].location);
        ParseExpressionOfComand(spawn.parameters, errors);
        return spawn;
    }
    private Color? ParseColor(List<CompilingError> errors)
    {
        Color color = new Color(tokens[currentPosition].location);
        ParseExpressionOfComand(color.parameters, errors);
        return color;
    }
    private Size ParseSize(List<CompilingError> errors)
    {
        Size size = new Size(tokens[currentPosition].location);
        ParseExpressionOfComand(size.parameters, errors);
        return size;
    }
    private DrawLine ParseDrawLine(List<CompilingError> errors)
    {
        DrawLine drawLine = new DrawLine(tokens[currentPosition].location);
        ParseExpressionOfComand(drawLine.parameters, errors);
        return drawLine;
    }
    private DrawCircle ParseDrawCircle(List<CompilingError> errors)
    {
        DrawCircle drawCircle = new DrawCircle(tokens[currentPosition].location);
        ParseExpressionOfComand(drawCircle.parameters, errors);
        return drawCircle;
    }
    private DrawRectangle ParseDrawRectangle(List<CompilingError> errors)
    {
        DrawRectangle drawRectangle = new DrawRectangle(tokens[currentPosition].location);
        ParseExpressionOfComand(drawRectangle.parameters, errors);
        return drawRectangle;
    }
    private Fill ParseFill(List<CompilingError> errors)
    {
        Fill fill = new Fill(tokens[currentPosition].location);
        ParseExpressionOfComand(fill.parameters, errors);
        return fill;
    }
#endregion
#region Parse Function
    private GetActualX ParseGetActualX(List<CompilingError> errors)
    {
        GetActualX getActualX = new GetActualX(tokens[currentPosition].location);
        ParseExpressionOfComand(getActualX.parameters, errors);
        return getActualX;
    }
    private GetActualY ParseGetActualY(List<CompilingError> errors)
    {
        GetActualY getActualY = new GetActualY(tokens[currentPosition].location);
        ParseExpressionOfComand(getActualY.parameters, errors);
        return getActualY;
    }
    private GetCanvasSize ParseGetCanvasSize(List<CompilingError> errors)
    {
        GetCanvasSize GetCanvasSize = new GetCanvasSize(tokens[currentPosition].location);
        ParseExpressionOfComand(GetCanvasSize.parameters, errors);
        return GetCanvasSize;
    }
    private GetColorCount ParseGetColorCount(List<CompilingError> errors)
    {
        GetColorCount GetColorCount = new GetColorCount(tokens[currentPosition].location);
        ParseExpressionOfComand(GetColorCount.parameters, errors);
        return GetColorCount;
    }
    private IsBrushColor ParseIsBrushColor(List<CompilingError> errors)
    {
        IsBrushColor IsBrushColor = new IsBrushColor(tokens[currentPosition].location);
        ParseExpressionOfComand(IsBrushColor.parameters, errors);
        return IsBrushColor;
    }
    private IsBrushSize ParseIsBrushSize(List<CompilingError> errors)
    {
        IsBrushSize IsBrushSize = new IsBrushSize(tokens[currentPosition].location);
        ParseExpressionOfComand(IsBrushSize.parameters, errors);
        return IsBrushSize;
    }
    private IsColor ParseIsColor(List<CompilingError> errors)
    {
        IsColor IsColor = new IsColor(tokens[currentPosition].location);
        ParseExpressionOfComand(IsColor.parameters, errors);
        return IsColor;
    }
#endregion
#region ParseExpression
    private Expression? ParseExpression()
    {
        return ParseExpressionL1(null);
    }
    private Expression? ParseExpressionL1(Expression? left)
    {
        Expression? newLeft = ParseExpressionL2(left);
        Expression? expression = ParseExpressionL1_(newLeft);
        return expression;
    }
    private Expression? ParseExpressionL1_(Expression? left)
    {
        Expression? exp = ParseAdd(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseSub(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }
    private Expression? ParseExpressionL2(Expression? left)
    {
        Expression? newLeft = ParseExpressionL3(left);
        Expression? expression = ParseExpressionL2_(newLeft);
        return expression;
    }
    private Expression? ParseExpressionL2_(Expression? left)
    {
        Expression? exp = ParseMul(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseDiv(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }
    private Expression? ParseExpressionL3(Expression? left)
    {
        Expression? expression = ParseNumber();
        if(expression != null)
        {
            return expression;
        }
        expression = ParseIdentifier();
        if(expression != null)
        {
            return expression;
        }
        return null;
    }
    private Expression? ParseAdd(Expression? left)
    {
        Add? sum = new Add(tokens[currentPosition].location);
        if(left == null)
        {
            return null;
        }
        sum.left = left;
        Consume(); // Skip the left token

        if(tokens[currentPosition].type != TokenType.Plus)
        {
            return null;
        }
        Consume(); // Skip the '+' token

        Expression? right = ParseExpressionL2(null);
        if(right == null)
        {
            currentPosition-=2; //Back to left token
            return null;
        }
        sum.right = right;

        return ParseExpressionL1_(sum);
    }
    private Expression? ParseSub(Expression? left)
    {
        Sub? sub = new Sub(tokens[currentPosition].location);
        if(left == null)
        {
            return null;
        }
        sub.left = left;
        Consume(); // Skip the left token

        if(tokens[currentPosition].type != TokenType.Minus)
        {
            return null;
        }
        Consume(); // Skip the '-' token

        Expression? right = ParseExpressionL2(null);
        if(right == null)
        {
            currentPosition-=2; //Back to left token
            return null;
        }
        sub.right = right;

        return ParseExpressionL1_(sub);
    }
    private Expression? ParseMul(Expression? left)
    {
        Mul? mul = new Mul(tokens[currentPosition].location);
        if(left == null)
        {
            return null;
        }
        mul.left = left;
        Consume(); // Skip the left token

        if(tokens[currentPosition].type != TokenType.Minus)
        {
            return null;
        }
        Consume(); // Skip the '*' token

        Expression? right = ParseExpressionL2(null);
        if(right == null)
        {
            currentPosition-=2; //Back to left token
            return null;
        }
        mul.right = right;

        return ParseExpressionL1_(mul);
    }
    private Expression? ParseDiv(Expression? left)
    {
        Div? div = new Div(tokens[currentPosition].location);
        if(left == null)
        {
            return null;
        }
        div.left = left;
        Consume(); // Skip the left token

        if(tokens[currentPosition].type != TokenType.Minus)
        {
            return null;
        }
        Consume(); // Skip the '/' token

        Expression? right = ParseExpressionL2(null);
        if(right == null)
        {
            currentPosition-=2; //Back to left token
            return null;
        }
        div.right = right;

        return ParseExpressionL1_(div);
    }
    private Expression? ParseNumber()
    {
        if(tokens[currentPosition].type != TokenType.Number)
            return null;
        return new Number(int.Parse(tokens[currentPosition].lexeme), tokens[currentPosition].location);
    }
    private Expression? ParseIdentifier()
    {
        if(tokens[currentPosition].type != TokenType.Identifier)
            return null;
        return new Identifier(int.Parse(tokens[currentPosition].lexeme), tokens[currentPosition].location);
    }
    private void ParseExpressionOfComand(List<Expression?> parameters, List<CompilingError> errors)
    {   
        Consume();
        if(tokens[currentPosition].type != TokenType.LeftParen)
            errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a '('"));
        Consume(); // Skip the '(' token

        for(int i = 0; i < parameters.Count; i++)
        {
            if(i >= 1)
            {
                if(tokens[currentPosition].type != TokenType.Comma)
                {
                    errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,$"Expected a ','"));
                }
                Consume(); // Skip the ',' token
            }
            Expression? newExpression = ParseExpression();
            if (newExpression == null)
            {
                errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Invalid,""));
            }
            parameters[i] = newExpression;
            Consume(); // Skip expression
        }

        Consume();
        if(tokens[currentPosition].type != TokenType.RightParen)
            errors.Add(new CompilingError(tokens[currentPosition].location,ErrorCode.Expected,"Expected a ')'"));
        Consume(); // Skip the ')' token
    }
#endregion
    private void Consume()
    {
        currentPosition++; //Skip current token != EOL
        while(tokens[currentPosition].type == TokenType.EndOfLine)
            currentPosition++;
    }
}