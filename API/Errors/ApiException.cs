// the response of what are we going to send back to the client whene we have an exception

namespace API.Errors
{
    public class ApiException
    {

        public ApiException(int statusCode, string message, string details){
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
        
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

        
    }
}