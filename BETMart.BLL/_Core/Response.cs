using System.Collections.Generic;
using System.Threading.Tasks;

namespace BETMart.BLL._Core
{
    public class Response<T>
        : Response
        where T : class
    {
        public T Data { get; set; }

        public static Response<T> Success(T data, string message)
        {
            return new Response<T>
            {
                IsSuccessful = true,
                Message = message,
                Data = data
            };
        }
        public new static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                IsSuccessful = false,
                Message = message
            };
        }
    }

    public class Response
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public static Response Success(string message)
        {
            return new Response
            {
                IsSuccessful = true,
                Message = message
            };
        }
        public static Response Fail(string message)
        {
            return new Response
            {
                IsSuccessful = false,
                Message = message
            };
        }
    }

    public class ErrorResponse<T>
        : Response<T>
        where T : class
    {
        public List<string> Errors { get; set; }
        public static ErrorResponse<T> Fail(string message, List<string> errors)
        {
            return new ErrorResponse<T>
            {
                IsSuccessful = false,
                Message = message,
                Errors = errors,
                Data = null
            };
        }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

    public interface IResult
    {
        string Message { get; set; }

        bool Succeeded { get; set; }
    }

    public class Result : IResult
    {
        public bool Failed => !this.Succeeded;

        public string Message { get; set; }

        public bool Succeeded { get; set; }

        public static IResult Fail() => (IResult)new Result()
        {
            Succeeded = false
        };

        public static IResult Fail(string message) => (IResult)new Result()
        {
            Succeeded = false,
            Message = message
        };

        public static Task<IResult> FailAsync() => Task.FromResult<IResult>(Result.Fail());

        public static Task<IResult> FailAsync(string message) => Task.FromResult<IResult>(Result.Fail(message));

        public static IResult Success() => (IResult)new Result()
        {
            Succeeded = true
        };

        public static IResult Success(string message) => (IResult)new Result()
        {
            Succeeded = true,
            Message = message
        };

        public static Task<IResult> SuccessAsync() => Task.FromResult<IResult>(Result.Success());

        public static Task<IResult> SuccessAsync(string message) => Task.FromResult<IResult>(Result.Success(message));
    }
    public class Result<T> : Result, IResult<T>, IResult
    {
        public T Data { get; set; }

        public static Result<T> Fail()
        {
            Result<T> result = new Result<T>();
            result.Succeeded = false;
            return result;
        }

        public static Result<T> Fail(string message)
        {
            Result<T> result = new Result<T>();
            result.Succeeded = false;
            result.Message = message;
            return result;
        }

        public static Task<Result<T>> FailAsync() => Task.FromResult<Result<T>>(Result<T>.Fail());

        public static Task<Result<T>> FailAsync(string message) => Task.FromResult<Result<T>>(Result<T>.Fail(message));

        public static Result<T> Success()
        {
            Result<T> result = new Result<T>();
            result.Succeeded = true;
            return result;
        }

        public static Result<T> Success(string message)
        {
            Result<T> result = new Result<T>();
            result.Succeeded = true;
            result.Message = message;
            return result;
        }

        public static Result<T> Success(T data)
        {
            Result<T> result = new Result<T>();
            result.Succeeded = true;
            result.Data = data;
            return result;
        }

        public static Result<T> Success(T data, string message)
        {
            Result<T> result = new Result<T>();
            result.Succeeded = true;
            result.Data = data;
            result.Message = message;
            return result;
        }

        public static Task<Result<T>> SuccessAsync() => Task.FromResult<Result<T>>(Result<T>.Success());

        public static Task<Result<T>> SuccessAsync(string message) => Task.FromResult<Result<T>>(Result<T>.Success(message));

        public static Task<Result<T>> SuccessAsync(T data) => Task.FromResult<Result<T>>(Result<T>.Success(data));

        public static Task<Result<T>> SuccessAsync(T data, string message) => Task.FromResult<Result<T>>(Result<T>.Success(data, message));
    }
}
