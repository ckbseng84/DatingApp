using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(10,MinimumLength=6, ErrorMessage=" 6 to 10 characters")]
        public string Password { get; set; }
    }
}