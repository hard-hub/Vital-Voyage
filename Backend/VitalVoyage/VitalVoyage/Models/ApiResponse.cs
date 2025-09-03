using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace VitalVoyage.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int SuccessCode { get; set; }    
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        // Success Constructor
        public ApiResponse(T data, int statusCode = 200, string message = "") 
        { 
            Success = true;
            SuccessCode = statusCode;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }
        // Failure Constructor
        public ApiResponse(int statusCode, string errorMessage, List<string> errors = null)
        {
            Success = false;
            SuccessCode = statusCode;
            Message = errorMessage;
            Data = default;
            Errors = errors ?? new List<string>();
        }
        // Helper methods
        public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200, string message = "")
        {
            return new ApiResponse<T>(data, statusCode, message);
        }
        public static ApiResponse<T> FailureResponse(int statusCode, string errorMessage, List<string> errors = null)
        {
            return new ApiResponse<T>(statusCode, errorMessage, errors);
        }
    }
    // For endpoints that do not return data
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int SuccessCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();

        public static ApiResponse SuccessResponse(string message = "", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                SuccessCode = statusCode,
                Message = message,
                Errors = new List<string>()
            };
        }
        public static ApiResponse FailureResponse(int statusCode, string errorMessage, List<string> errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                SuccessCode = statusCode,
                Message = errorMessage,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
