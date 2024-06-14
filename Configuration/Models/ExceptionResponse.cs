namespace Configuration.Models
{
    public sealed class ExceptionResponse
    {
        public string Title { get; set; } = String.Empty;

        public string Message { get; set; } = String.Empty;

        public int StatusCode { get; set; }
    }
}