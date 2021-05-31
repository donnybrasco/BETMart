using System.Collections.Generic;

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
}
