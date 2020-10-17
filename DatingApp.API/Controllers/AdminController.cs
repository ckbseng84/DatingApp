using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly IOptions<CloudinarySettings> _cloudinarySetting;
        public AdminController(DataContext context, UserManager<User> userManager,
                            IDatingRepository repo, IMapper mapper, 
                            IOptions<CloudinarySettings> cloudinarySetting)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _repo = repo;
            _cloudinarySetting = cloudinarySetting;
             Account acc = new Account(
                _cloudinarySetting.Value.CloudName,
                _cloudinarySetting.Value.ApiKey,
                _cloudinarySetting.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);

        }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("usersWithRoles")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var userList = await _context.Users
            .OrderBy(x => x.UserName)
            .Select(user => new
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (from userRole in user.UserRoles
                         join role in _context.Roles
                         on userRole.RoleId equals role.Id
                         select role.Name).ToList()
            }).ToListAsync();
        return Ok(userList);
    }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("editRoles/{userName}")]
    public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var userRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = roleEditDto.RoleNames;

        // something like this > selected = selected != null ? selectedRoles : new string[]{}
        selectedRoles = selectedRoles ?? new string[] { };

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded)
            return BadRequest("Failed to add to roles");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded)
            return BadRequest("Failed to remove roles");

        return Ok(await _userManager.GetRolesAsync(user));

    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photosForModeration")]
    public async Task<IActionResult> getPhotosForModeration()
    {
        var photoForModeration = await _context.Photos.Where(x => x.IsApproved == false).ToListAsync();
        if (photoForModeration == null)
            return NotFound();
        return Ok(_mapper.Map<IEnumerable<PhotoForModerateDto>>(photoForModeration));

    }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("photosForModeration/{id}/setApprove")]
    public async Task<IActionResult> SetApprovePhoto(int id)
    {
        var photoFromRepo = await _repo.GetPhoto(id);

        if (photoFromRepo == null)
            return BadRequest("Photo not found");

        if (photoFromRepo.IsApproved)
            return BadRequest("This photo already approved");

        photoFromRepo.IsApproved = true;

        if (await _repo.SaveAll())
            return NoContent();

        return BadRequest("unable to approve photo");
    }
    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("photosForModeration/{id}/setReject")]
    public async Task<IActionResult> SetPhotoReject(int id)
    {
        if (!_context.Photos.Any(p => p.Id == id))
            return Unauthorized("photo not found");

        var photoFromRepo = await _repo.GetPhoto(id);

        if (photoFromRepo.PublicId != null)
        {
            var deleteParams = new DeletionParams(photoFromRepo.PublicId);
            var result = _cloudinary.Destroy(deleteParams);

            if (result.Result != "ok")
                return BadRequest("unable to delete photo at cloud");

        }
        
        _repo.Delete(photoFromRepo);
        if (await _repo.SaveAll())
            return Ok();
        return BadRequest("unable to reject photo");
    
    }
}
}