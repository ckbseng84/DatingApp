using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        PhotoForCreationDto()
        {
            DateAdded=DateTime.Now;
        }
        public string Url { get; set; }
        public IFormFile File { get; set; } // TODO : IFormFile
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }    
        public string PublicId { get; set; }
    }

}