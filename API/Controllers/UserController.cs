using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{

[Authorize]
public class UserController : BaseApiController
{
    
    private readonly DataContext _context;
    public UserController(DataContext context)
    {
        _context = context;
    }
    [AllowAnonymous]
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