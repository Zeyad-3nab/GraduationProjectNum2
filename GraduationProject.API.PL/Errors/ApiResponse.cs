using System.Net;

namespace GraduationProject.API.PL.Errors
{
    public class ApiResponse
    {
        public List<string>? errors { get; set; } = new List<string>();
        public HttpStatusCode statusCode { get; set; } = HttpStatusCode.BadRequest;
        public object? message { get; set; } = null;
    }
}
