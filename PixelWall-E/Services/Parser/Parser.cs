using System.Linq.Expressions;

public class Parser
{
    private List<Token>? tokens;
    private int currentPosition;

    Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    /*public Spawn ParseSpawn(List<CompilingError> errors)
    {
        Spawn()
    }*/
}