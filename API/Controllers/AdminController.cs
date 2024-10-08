using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            
        }
        [Authorize(Policy = "RequireAdiminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles(){

            var users = await _userManager.Users
                        .OrderBy(u => u.UserName)
                        .Select(u => new{
                            u.Id,
                            Username = u.UserName,
                            Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                        })
                        .ToListAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdiminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles){
            
            if(string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }


        [Authorize(Policy = "RequireAdiminRole")]
        [HttpDelete("delete-user/{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(new { Message = "User not found." });

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(new { Message = "Failed to delete user." });

            return Ok(new { Message = "User deleted successfully." });
        }



        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("Photos-to-moderate")]
        public ActionResult GetPhotosForModeration(){
            return Ok("Admin or moderators can see this");
        }
        
    }
}