using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto user)
        {
            //validate request, will be covered later
            user.UserName= user.UserName.ToLower();

            if(await _repo.UserExists(user.UserName))
                return BadRequest("User name already exists!");
            
            var newUser = new User
            {
                UserName=user.UserName
            };
            
            var createduser = await _repo.Register(newUser,user.Password);

            return StatusCode(201);

        }
    }
}