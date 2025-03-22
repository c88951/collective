using System.Collections.Generic;
using System.Net;

namespace CollectiveData.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public PaginationMetadata? Pagination { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static ApiResponse<T> Created(T data, string message = "Created successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = HttpStatusCode.Created,
                Data = data
            };
        }

        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static ApiResponse<T> BadRequest(string message = "Invalid request", List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = errors
            };
        }

        public static ApiResponse<T> ServerError(string message = "Internal server error")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static ApiResponse<T> Unauthorized(string message = "Unauthorized access")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCode.Unauthorized
            };
        }
    }

    public class PaginationMetadata
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
