namespace NotesApi.Common;

// -->> for specific tyrpe data response
public class ApiResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string StatusType { get; set; } = "OK"; 
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = StatusCodes.Status200OK)
    {
        return new ApiResponse<T>
        {
            Message = message,
            StatusCode = statusCode,
            StatusType = "OK",
            Data = data
        };
    }

    public static ApiResponse<T> Error(string message, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ApiResponse<T>
        {
            Message = message,
            StatusCode = statusCode,
            StatusType = "Error",
            Data = default
        };
    }
}

// -->>> for specific unspecific type data response , example : null
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string message = "Success", int statusCode = StatusCodes.Status200OK)
    {
        return new ApiResponse
        {
            Message = message,
            StatusCode = statusCode,
            StatusType = "OK",
            Data = null
        };
    }

    public static new ApiResponse Error(string message, int statusCode = StatusCodes.Status400BadRequest)
    {
        return new ApiResponse
        {
            Message = message,
            StatusCode = statusCode,
            StatusType = "Error",
            Data = null
        };
    }
}
