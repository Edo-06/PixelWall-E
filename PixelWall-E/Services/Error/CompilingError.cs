public class CompilingError
    {
        public ErrorCode code { get; private set; }

        public string message { get; private set; }

        public CodeLocation location {get; private set;}

        public CompilingError(CodeLocation location, ErrorCode code, string message)
        {
            this.code = code;
            this.message = message;
            this.location = location;
        }
    }

    public enum ErrorCode
    {
        Expected,
        Invalid,
        Unknown,
    }