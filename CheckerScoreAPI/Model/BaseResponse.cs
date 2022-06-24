namespace CheckerScoreAPI.Model
{
    public static class BaseResponse
    {
        public static BaseResponse<T> GetResponse<T>(bool success, string message, T? result = null) where T: class
        {
            return new BaseResponse<T>()
            {
                Message = message,
                Result = result,
                Success = success
            };
        }
    }

    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}