using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/user  ---->  // i can replace it to any custom path
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    public UserController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("{id}")] // api/user/2
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}




// [HttpGet]
//     public ActionResult<IEnumerable<AppUser>> GetUsers()
//     {
//         return _context.Users.ToList();
//     }

//     [HttpGet("{id}")] // api/user/2
//     public ActionResult<AppUser> GetUser(int id)
//     {
//         return _context.Users.Find(id);
//     }