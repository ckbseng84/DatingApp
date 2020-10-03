using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LoginUserActivity))]
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet("{id}",Name="GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (!IsAuthorizedUser(userId)) return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)
                return NotFound();
            
            return Ok(messageFromRepo);
            
        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery] MessageParams messageParams)
        {
            if (!IsAuthorizedUser(userId)) return Unauthorized();
            messageParams.UserId = userId;
            var messageFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

            Response.AddPagination(messageFromRepo.CurrentPage
                                    , messageFromRepo.PageSize
                                    , messageFromRepo.TotalCount
                                    , messageFromRepo.TotalPages);
            return Ok(messages);
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageforCreationDto messageforCreationDto)
        {
            if (!IsAuthorizedUser(userId)) return Unauthorized();

            messageforCreationDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageforCreationDto.RecipientId);

            if(recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageforCreationDto);

            _repo.Add(message);

            

            if(await _repo.SaveAll())  
            {
                var messageToReturn = _mapper.Map<MessageforCreationDto>(message);
                return CreatedAtRoute("GetMessage",new {userId, id = message.Id}, messageToReturn);
            }
            throw new System.Exception("Create this message failed on save");   

        }
        private bool IsAuthorizedUser(int userId)
        {
            return (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }
        

    }
}