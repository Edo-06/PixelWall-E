public class Callable
{
    public TokenPattern tokenPattern {get; private set;} = null!;
    public Expected expected {get; private set;} = null!;

    public Callable(TokenPattern tokenPattern, Expected expected)
    {
        this.tokenPattern = tokenPattern;
        this.expected = expected;
    }
}