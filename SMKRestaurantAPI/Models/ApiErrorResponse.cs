namespace SMKRestaurantAPI.Models
{
    /// <summary>
    /// Format error response standar untuk seluruh endpoint API.
    /// </summary>
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "Terjadi kesalahan.";
        public string? Detail { get; set; }
        public string? TraceId { get; set; }
    }
}
