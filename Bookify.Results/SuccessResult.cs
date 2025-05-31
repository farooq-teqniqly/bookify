namespace Bookify.Results;

public sealed class SuccessResult<T> : Result<T>
{
    private readonly T _value;

    public SuccessResult(T value) => _value = value;

    public override Error GetError() => null!;

    public override T GetValue() => _value;
}
