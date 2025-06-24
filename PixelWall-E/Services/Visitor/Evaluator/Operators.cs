public static class Operators<T> where T : IComparable<T>
{
    public static Dictionary<TokenType, Func<int, int, int>> AritmeticOperator = new Dictionary<TokenType, Func<int, int, int>>
    {
        { TokenType.Plus, (a, b) => a + b},
        { TokenType.Minus, (a, b) => a - b},
        { TokenType.Multiply, (a, b) => a * b},
        { TokenType.Divide, (a, b) => 
            {
                if (b == 0) throw new DivideByZeroException("");
                return a / b;
            }
        },
        { TokenType.Power, (a, b) => (int)Math.Pow(a, b)},
        { TokenType.Modulo, (a, b) =>
        {
            if (b == 0) throw new DivideByZeroException("modulo by zero is not allowed"); 
            return a % b;
        }
    }
    };
    public static Dictionary<TokenType, Func<T, T, bool>> ComparisionOperator = new Dictionary<TokenType, Func<T, T, bool>>
    {
        { TokenType.Equal, (a, b) => a.Equals(b) },
        { TokenType.NotEqual, (a, b) => !a.Equals(b) },
        { TokenType.Less, (a, b) => Comparer<T>.Default.Compare(a, b) < 0 },
        { TokenType.Greater, (a, b) => Comparer<T>.Default.Compare(a, b) > 0 },
        { TokenType.LessEqual, (a, b) => Comparer<T>.Default.Compare(a, b) <= 0 },
        { TokenType.GreaterEqual, (a, b) => Comparer<T>.Default.Compare(a, b) >= 0 }
    };
    public static Dictionary<TokenType, Func<bool, bool, bool>> BooleanOperator = new Dictionary<TokenType, Func<bool, bool, bool>>
    {
        { TokenType.And, (a, b) => a && b },
        { TokenType.Or, (a, b) => a || b }
    };
}