

using Infrastracture.Models;
using Infrastructure.Abstractions.Errors;


namespace Infrastructure.Abstractions;
public class Result
{
    public Error? Error { get; }
    public bool IsSuccess => Error == null;

    protected Result(Error? error)
    {
        Error = error;
    }

    public static Result Success() => new Result(null);

    public static Result Failure(Error error) => new Result(error);

    public TResult Map<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(null)
    {
        Value = value;
    }

    private Result(Error error) : base(error)
    {
        Value = default;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static new Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }

    public TResult Map<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
      
}


