// this middleware is going to catch the exception that are thrown in the application and then it is going to log the exception in the terminal and then it is going to return a response to the client with the status code and the message of the exception that was thrown.
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger, 
            IHostEnvironment env)
        {
            // next : because it is middleware
            // logger : to log the exception, we are going to log the exception in the terminal
            // env : allows us to know if we are running in development mode or in production mode.
            _next = next;
            _logger = logger;
            _env = env;

        }
        public async Task InvokeAsync(HttpContext context){
            try{
                await _next(context);
            }catch(Exception ex){
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "applicatio/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);

            }
        }
        
    }
}