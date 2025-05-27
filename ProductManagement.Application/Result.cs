using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application
{
    public record Result<T>
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }

        public static Result<T> Ok(T data, string? message = null) =>
            new Result<T> { Success = true, Data = data, Message = message };

        public static Result<T> Fail(string message) =>
            new Result<T> { Success = false, Message = message };
    }
}
