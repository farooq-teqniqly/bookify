namespace Bookify.Results
{
    public abstract class Error
    {
        protected Error(string code, string message, IDictionary<string, object>? errorData = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(code);
            ArgumentException.ThrowIfNullOrEmpty(message);

            Code = code;
            Message = message;
            ErrorData = errorData;
        }

        public string Code { get; }
        public IDictionary<string, object>? ErrorData { get; }
        public string Message { get; }
    }
}
