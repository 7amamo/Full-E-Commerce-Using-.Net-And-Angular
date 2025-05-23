using Microsoft.AspNetCore.Http;

namespace ECom.Api.Helper
{
    public class ResponseApi
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ResponseApi(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(StatusCode);
        }

        private string? GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200  => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                500 => "server Error",
                _ => null,
            };
        }
    }
}
