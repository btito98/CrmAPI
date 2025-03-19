namespace CRM.Shared.Results;

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error)
    {
        Value = default;
    }
    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);
}