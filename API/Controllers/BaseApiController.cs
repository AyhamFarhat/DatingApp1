
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
[ServiceFilter(typeof(LogUserActivity))] // this will be applied to all controllers
[ApiController]
[Route("api/[controller]")] // /api/user  ---->  // i can replace it to any custom path
    public class BaseApiController : ControllerBase
    {
        
        
    }
}