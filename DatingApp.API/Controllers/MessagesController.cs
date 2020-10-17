using System;
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
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (!IsAuthorizedUser(userId)) return Unauthorized();

            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageforCreationDto messageforCreationDto)
        {
            if (!IsAuthorizedUser(userId)) return Unauthorized();

            var sender = await _repo.GetUser(userId, true);// Automapper - magic map
            var recipient = await _repo.GetUser(messageforCreationDto.RecipientId,false);

            if(recipient == null)
                return BadRequest("Could not find user");

            messageforCreationDto.SenderId = userId;

            var message = _mapper.Map<Message>(messageforCreationDto);
            
            _repo.Add(message);

            if(await _repo.SaveAll())  
            {
                var messageToReturnDto = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage",new {userId, id = message.Id}, messageToReturnDto);
                
            }
            throw new System.Exception("Create this message failed on save");   

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (!IsAuthorizedUser(userId))return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);
            
            if (await _repo.SaveAll())
                return NoContent();
            throw new System.Exception("Error deleting the message");

        }
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (!IsAuthorizedUser(userId))return Unauthorized();
             var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.RecipientId != userId)
            {
                return Unauthorized();
            }

            messageFromRepo.IsRead=true;
            messageFromRepo.DateRead = DateTime.Now;
            
            await _repo.SaveAll();
            
            return NoContent();

        }

        private bool IsAuthorizedUser(int userId)
        {
            return (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }
        

    }
}