using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers{

[Authorize]
public class UserController : BaseApiController
{
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;

    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();
        return Ok(users);
    }


    [HttpGet("{username}")] // api/user/2
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await _userRepository.GetMemberAsync(username);
        return user;
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