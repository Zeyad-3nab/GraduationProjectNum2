namespace GraduationProject.API.PL.Errors
{
	public class ApiErrorResponse
	{
	    // Response دي الطريقه اللي بنهندل بيها عشان نوحد بيها شكل ال
	    public int StatusCode { get; set; }
	    public string? Message { get; set; }
	    public ApiErrorResponse(int statusCode, string? message = null)
	    {
	    	StatusCode = statusCode;
	    	Message = message ?? GetDefaultMessageForStatusCode(statusCode); //GetDefaultMessageForStatusCode هنا بقوله لو مفيش رساله جاتلك اعمل جنيريت للميثود دي
	    }
	    
	    private string? GetDefaultMessageForStatusCode(int statusCode)
	    {
	    	var message = statusCode switch //New Feature => C# 7
	    	{
	    		400 => "a bad Request , You have made",
	    		401 => "Authorized , your not found",
	    		404 => "Resource was not found",
	    		500 => "Server Error",
	    		_ => null
	    	};
	    	return message;
	    }
	}
}
