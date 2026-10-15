namespace RDsport.ApiResponse
{
    public class ApiResponseClass<TData>
    {

        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public object? Errors { get; set; }
        public string? Message { get; set; } = string.Empty;
        public TData? data { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


        public static ApiResponseClass<TData> Create(int statusCode, bool success, object? errors = default, string? message=default, TData? data=default)
        {
            return new ApiResponseClass<TData>()
            {
                StatusCode = statusCode,
                Success = success,
                Errors = errors,
                Message = message,
                data = data

            };
        }

        public static ApiResponseClass<TData> NotFound(string message, object? errors) => Create( 404, false, message:message, errors: errors);
        public static ApiResponseClass<TData> NoContent(string message, TData data) => Create( 204, true, data:data);
        public static ApiResponseClass<TData> Ok(string message, TData? data) => Create( 200, true, message:message, data:data);
        public static ApiResponseClass<TData> CreatedAt(string message, TData data) => Create( 201, true, message, data:data);
        public static ApiResponseClass<TData> Conflict(string message,object? errors) => Create( 409, true, errors: errors, message);
        public static ApiResponseClass<TData> BadRequest(string message, object errors) => Create( 400, false, message:message, errors: errors);

    }

    
}
