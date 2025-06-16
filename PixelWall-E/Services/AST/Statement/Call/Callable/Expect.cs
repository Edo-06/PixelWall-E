public class Expected
{
    public List<ExpressionType> Input {get; private set;}= [];
    public ExpressionType Output {get; private set;}
    public Expected(List<ExpressionType> input, ExpressionType output)
    {
        Input = input;
        Output = output;
    }
}