namespace SMKRestaurantAPI.Models
{
    /// <summary>
    /// Envelope response sukses standar untuk seluruh endpoint.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "OK";
        public T? Data { get; set; }
        public int? Count { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "OK", int? count = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Count = count
            };
        }
    }
}
