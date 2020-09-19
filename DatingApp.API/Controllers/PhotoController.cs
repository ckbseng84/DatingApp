using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySetting;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinarySetting) //inject specifically by group
        {
            _cloudinarySetting = cloudinarySetting;
            _repo = repo;
            _mapper = mapper;
            // cloudinary account initialization
            Account acc = new Account(
                _cloudinarySetting.Value.CloudName, 
                _cloudinarySetting.Value.ApiKey,
                _cloudinarySetting.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhotos(int Id){
           
            var photoFromRepo = await _repo.GetPhoto(Id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
                            [FromForm]PhotoForCreationDto photoForCreationDto){
            // verify useer id against with claim 
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(userId);
             // form-data for files            
             var file = photoForCreationDto.File;

             // cloudinary object
            var uploadResult = new ImageUploadResult(); 

            // verify file before upload
            if(file.Length>0){
                using( var stream = file.OpenReadStream()){
                    var uploadParams = new ImageUploadParams(){
                        File= new FileDescription(file.Name, stream), 
                        // cloudinary transformation
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    // call to upload
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            // bind url and publicid response from cloudinary
            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;
            
            userFromRepo.Photos.Add(photo);
            
            if(await _repo.SaveAll()){
                var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                // not sure what this line use for
                return CreatedAtRoute("GetPhoto",new{ Id = photo.Id, userId = userId},
                    photoForReturn);
            }

            return BadRequest("cannot add photo");

        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id){
            
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized("user id not match with token");
            
            var user = await _repo.GetUser(userId);
            
            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized("photo not found");
            
            var photoFromRepo = await _repo.GetPhoto(id);
            
            if(photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhoto(userId);

            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();
            return BadRequest("unable to set main photo");
        }
    }
}