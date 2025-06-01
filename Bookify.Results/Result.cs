namespace Bookify.Results
{
    public static class Result
    {
        public static Result<T> Failure<T>(Error error) => new ErrorResult<T>(error);

        public static Result<T> Success<T>(T value) => new SuccessResult<T>(value);
    }

    public abstract class Result<T>
    {
        public bool IsFailure => this is ErrorResult<T>;
        public bool IsSuccess => this is SuccessResult<T>;
        public abstract Error GetError();
        public abstract T GetValue();
    }
}
