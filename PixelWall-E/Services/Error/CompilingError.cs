public class CompilingError
    {
        public ErrorCode Code { get; private set; }

        public string Argument { get; private set; }

        public CodeLocation Location {get; private set;}

        public CompilingError(CodeLocation location, ErrorCode code, string argument)
        {
            this.Code = code;
            this.Argument = argument;
            Location = location;
        }
    }

    public enum ErrorCode
    {
        Expected,
        Invalid,
        Unknown,
    }