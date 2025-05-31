namespace Bookify.Results
{
    public abstract class Error
    {
        protected Error(string code, string message, IDictionary<string, object>? errorData = null)
        { }
    }
}
