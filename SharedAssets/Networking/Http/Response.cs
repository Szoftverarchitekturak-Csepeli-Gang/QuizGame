using System;

namespace Assets.SharedAssets.Networking.Http
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int? StatusCode { get; set; }

        public static Response<T> Success(T data) =>
            new() { IsSuccess = true, Data = data };

        public static Response<T> Failure(string message, int? statusCode = null) =>
            new() { IsSuccess = false, ErrorMessage = message, StatusCode = null };

    }
}
