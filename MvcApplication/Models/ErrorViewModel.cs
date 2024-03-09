namespace MvcApplication.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? ErrorCode { get; set; }
        public bool ShowErrorCode => !string.IsNullOrEmpty(ErrorCode);
        public string? ErrorMessage { get; set; }
        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    }
}
