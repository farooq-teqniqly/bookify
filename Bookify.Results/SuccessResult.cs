namespace Bookify.Results;

public sealed class SuccessResult<T> : Result<T>
{
    private readonly T _value;

    public SuccessResult(T value) => _value = value;

    public override Error GetError() =>
        throw new InvalidOperationException("Cannot get error from a successful result.");

    public override T GetValue() => _value;
}
