namespace CheckerScoreAPI.Model
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Result { get; set; }

        public BaseResponse(bool success, string message, object? result = null)
        {
            Success = success;
            Message = message;
            Result = result;
        }
    }
}