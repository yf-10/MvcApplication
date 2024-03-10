namespace MvcApplication.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowErrorCode => !string.IsNullOrEmpty(ErrorCode);
        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    }
}
