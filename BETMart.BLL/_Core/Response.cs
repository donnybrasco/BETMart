using System.Collections.Generic;
using System.Threading.Tasks;

namespace BETMart.BLL._Core
{
    // public class Response<T>
    //     : Response
    //     where T : class
    // {
    //     public T Data { get; set; }
    //
    //     public static Response<T> Success(T data, string message)
    //     {
    //         return new Response<T>
    //         {
    //             IsSuccessful = true,
    //             Message = message,
    //             Data = data
    //         };
    //     }
    //     public new static Response<T> Fail(string message)
    //     {
    //         return new Response<T>
    //         {
    //             IsSuccessful = false,
    //             Message = message
    //         };
    //     }
    // }
    //
    // public class Response
    // {
    //     public bool IsSuccessful { get; set; }
    //     public string Message { get; set; }
    //     public static Response Success(string message)
    //     {
    //         return new Response
    //         {
    //             IsSuccessful = true,
    //             Message = message
    //         };
    //     }
    //     public static Response Fail(string message)
    //     {
    //         return new Response
    //         {
    //             IsSuccessful = false,
    //             Message = message
    //         };
    //     }
    // }

    public interface IResponse<out T> : IResponse
    {
        T Data { get; }
    }

    public interface IResponse
    {
        string Message { get; set; }

        bool IsSuccessful { get; set; }
    }

    public class Response : IResponse
    {
        public bool Failed => !this.IsSuccessful;

        public string Message { get; set; }

        public bool IsSuccessful { get; set; }

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

        public static IResponse Fail() => (IResponse)new Response()
        {
            IsSuccessful = false
        };
        
        public static Task<IResponse> FailAsync() => Task.FromResult<IResponse>(Response.Fail());

        public static Task<IResponse> FailAsync(string message) => Task.FromResult<IResponse>(Response.Fail(message));

        public static IResponse Success() => (IResponse)new Response()
        {
            IsSuccessful = true
        };
        
        public static Task<IResponse> SuccessAsync() => Task.FromResult<IResponse>(Response.Success());

        public static Task<IResponse> SuccessAsync(string message) => Task.FromResult<IResponse>(Response.Success(message));
    }
    public class Response<T> : Response, IResponse<T>, IResponse
    {
        public T Data { get; set; }

        public static Response<T> Fail()
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = false;
            return response;
        }

        public static Response<T> Fail(string message)
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = false;
            response.Message = message;
            return response;
        }

        public static Task<Response<T>> FailAsync() => Task.FromResult<Response<T>>(Response<T>.Fail());

        public static Task<Response<T>> FailAsync(string message) => Task.FromResult<Response<T>>(Response<T>.Fail(message));

        public static Response<T> Success()
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = true;
            return response;
        }

        public static Response<T> Success(string message)
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = true;
            response.Message = message;
            return response;
        }

        public static Response<T> Success(T data)
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = true;
            response.Data = data;
            return response;
        }

        public static Response<T> Success(T data, string message)
        {
            Response<T> response = new Response<T>();
            response.IsSuccessful = true;
            response.Data = data;
            response.Message = message;
            return response;
        }

        public static Task<Response<T>> SuccessAsync() => Task.FromResult<Response<T>>(Response<T>.Success());

        public static Task<Response<T>> SuccessAsync(string message) => Task.FromResult<Response<T>>(Response<T>.Success(message));

        public static Task<Response<T>> SuccessAsync(T data) => Task.FromResult<Response<T>>(Response<T>.Success(data));

        public static Task<Response<T>> SuccessAsync(T data, string message) => Task.FromResult<Response<T>>(Response<T>.Success(data, message));
    }
}
