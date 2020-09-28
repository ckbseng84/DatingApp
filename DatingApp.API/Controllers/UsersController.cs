using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Models;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LoginUserActivity))]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // default => api/users
        // specific in url => api/users?pagenumber=2 or api/users?pagesize=5 or api/users?pagenumber=2&pagesize=5
        // return in header => {"currentPage":x,"itemsPerPage":x,"totalItems":x,"totalPages":x}
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams){
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            
            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);
            var mappedUsers = _mapper.Map<IEnumerable<UserForListDto>>(users);
            
            Response.AddPagination(users.CurrentPage, users.PageSize, 
                                    users.TotalCount, users.TotalPages);
            
            return Ok(mappedUsers);
        }
        [HttpGet("{id}",Name = "GetUser")]
        public async Task<IActionResult> GetUser(int Id){
            var user = await _repo.GetUser(Id);
            var mappedUser = _mapper.Map<UserForDetailDto>(user);
            return Ok(mappedUser);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto){
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(id);
            if (userFromRepo == null)
                throw new System.Exception($"user {id} not found!");

            _mapper.Map(userForUpdateDto, userFromRepo);
            if (await _repo.SaveAll())
                return NoContent(); 
            

            throw new System.Exception($"Updating user {id} failed on save");
        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId){
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
                return BadRequest("you have like this user");

            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like {
                LikerId = id,
                LikeeId = recipientId
            };
            _repo.Add<Like>(like);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }

    }
}