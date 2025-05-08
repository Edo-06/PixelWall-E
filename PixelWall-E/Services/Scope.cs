public static class Scope
{
    public static Dictionary<(string, int), object?> variables = new Dictionary<(string, int), object?>();
    public static Dictionary<string, int> labels = new Dictionary<string, int>();
}