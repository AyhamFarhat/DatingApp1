﻿// this file is used to create the database schema, to apply the schema to the database.
// this file handles the migration of the database schema.
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers{

    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;
            public UserController( IPhotoService photoService, IMapper mapper, IUnitOfWork uow)
        {
            _photoService = photoService;
            _uow = uow;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            // var currentUser= await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            // userParams.CurrentUsername = currentUser.UserName;
            var gender = await _uow.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUsername = User.GetUsername();

            if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender = gender == "male"?"male":"female";
            }

            var users = await _uow.UserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,
             users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
            // var users = await _uow.UserRepository.GetUsersAsync();
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);
        }

        [HttpGet("{username}")] // api/user/2
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _uow.UserRepository.GetMemberAsync(username);
            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            //_uow.UserRepository.Update(user);

            if (await _uow.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _uow.Complete())
            {
                return CreatedAtAction(nameof(GetUser),
                    new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        } 

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if(await _uow.Complete()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _uow.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
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