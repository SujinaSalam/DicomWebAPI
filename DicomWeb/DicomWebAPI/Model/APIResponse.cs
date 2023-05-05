using System.Net;

namespace DicomWebAPI.Model
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode {  get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
    }
}
