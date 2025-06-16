public static class ParameterChecker
{
    public static bool CheckParameters<T>(T node, IVisitor<bool> visitor, List<Exception> errors, CodeLocation location) where T: ICallableNode
    {
        List<ExpressionType> toCheck = [];
        if(node is CommandNode) toCheck = TypeOfCommandParameters[node.tokenType];
        if(node is FunctionNode) toCheck = TypeOfFunctionParameters[node.tokenType];
        for(int i = 0; i < toCheck.Count; i++)
        {
            if(!node.parameters[i].Accept(visitor)) return false;
            if(node.parameters[i].type != toCheck[i])
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Parameter {i + 1} must be of type {toCheck[i]} in node {node.tokenType}"));
                return false;
            }
        }
        return true;
    }
#region Commands
    private static List<ExpressionType> SpawnTypes = new List<ExpressionType>
    {
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> MoveToTypes = new List<ExpressionType>
    {
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> ColorTypes= new List<ExpressionType>
    {
        ExpressionType.Color
    };
    private static List<ExpressionType> SizeTypes = new List<ExpressionType>
    {
        ExpressionType.Number
    };
    private static List<ExpressionType>  DrawCircleTypes= new List<ExpressionType>
    {
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> DrawLineTypes = new List<ExpressionType>
    {
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> DrawRectangleTypes = new List<ExpressionType>
    {
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> FillTypes = new List<ExpressionType>{};
    private static Dictionary<TokenType, List<ExpressionType>> TypeOfCommandParameters = new Dictionary<TokenType, List<ExpressionType>>
    {
        { TokenType.Spawn, SpawnTypes },
        { TokenType.MoveTo, MoveToTypes },
        { TokenType.Color, ColorTypes },
        { TokenType.Size, SizeTypes },
        { TokenType.DrawCircle, DrawCircleTypes },
        { TokenType.DrawLine, DrawLineTypes },
        { TokenType.DrawRectangle, DrawRectangleTypes },
        { TokenType.Fill, FillTypes }
    };
#endregion
#region Functions
    private static List<ExpressionType> GetActualXTypes = new List<ExpressionType>{};
    private static List<ExpressionType> GetActualYTypes = new List<ExpressionType>{};
    private static List<ExpressionType> GetCanvasSizeTypes = new List<ExpressionType>{};
    private static List<ExpressionType> GetColorCountTypes = new List<ExpressionType>
    {
        ExpressionType.Color,
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static List<ExpressionType> IsBrushColorTypes = new List<ExpressionType>
    {
        ExpressionType.Color
    };
    private static List<ExpressionType> IsBrushSizeTypes = new List<ExpressionType>
    {
        ExpressionType.Number
    };
    private static List<ExpressionType> IsCanvasColorTypes = new List<ExpressionType>
    {
        ExpressionType.Color,
        ExpressionType.Number,
        ExpressionType.Number
    };
    private static Dictionary<TokenType, List<ExpressionType>> TypeOfFunctionParameters = new Dictionary<TokenType, List<ExpressionType>>
    {
        { TokenType.GetActualX, GetActualXTypes },
        { TokenType.GetActualY, GetActualYTypes },
        { TokenType.GetCanvasSize, GetCanvasSizeTypes },
        { TokenType.GetColorCount, GetColorCountTypes },
        { TokenType.IsBrushColor, IsBrushColorTypes },
        { TokenType.IsBrushSize, IsBrushSizeTypes },
        { TokenType.IsCanvasColor, IsCanvasColorTypes }
    };
    public static Dictionary<TokenType, ExpressionType> TypeOfFunctionReturn = new Dictionary<TokenType, ExpressionType>
    {
        { TokenType.GetActualX, ExpressionType.Number },
        { TokenType.GetActualY, ExpressionType.Number },
        { TokenType.GetCanvasSize, ExpressionType.Number },
        { TokenType.GetColorCount, ExpressionType.Number },
        { TokenType.IsBrushColor, ExpressionType.Bool },
        { TokenType.IsBrushSize, ExpressionType.Bool },
        { TokenType.IsCanvasColor, ExpressionType.Bool }
    };
#endregion
}