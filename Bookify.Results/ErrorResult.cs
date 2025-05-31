namespace Bookify.Results
{
    public sealed class ErrorResult<T> : Result<T>
    {
        private readonly Error _error;

        public ErrorResult(Error error) => _error = error;

        public override Error GetError() => _error;

        public override T GetValue() => default!;
    }
}
